using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using AutoMapper;
using Checkout.Service.Worker.Currency;
using Checkout.Service.Worker.Invoice;
using Checkout.Service.Worker.Models;
using Checkout.Service.Worker.Services;
using Checkout.Service.Worker.Timeline;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenTracing;

namespace Checkout.Service.Worker
{
    public class StartCheckoutWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StartCheckoutWorker> _logger;
        private readonly ITracer _tracer;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly AmazonSQSClient _amazonSQSClient;

        public StartCheckoutWorker(
            IConfiguration configuration,
            ILogger<StartCheckoutWorker> logger,
            ITracer tracer,
            IMediator mediator,
            IMapper mapper,
            AmazonSQSClient amazonSQSClient)
        {
            _configuration = configuration;
            _logger = logger;
            _tracer = tracer;
            _mediator = mediator;
            _mapper = mapper;
            _amazonSQSClient = amazonSQSClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var receiveMessageResponse = await GetMessagesFromSQS(stoppingToken);

                foreach (var sqsMessage in receiveMessageResponse.Messages)
                {
                    using (var scope = _tracer.BuildSpan("ProcessingMessage").StartActive(finishSpanOnDispose: true))
                    using (var ctx = _logger.BeginScope(new Dictionary<string, string>() { { "ReceiptHandle", sqsMessage.ReceiptHandle } }))
                    {
                        try
                        {
                            await ConsumeMessage(sqsMessage, scope);

                            await _amazonSQSClient.DeleteMessageAsync(new DeleteMessageRequest()
                            {
                                QueueUrl = _configuration["AmazonSQSCheckoutQueueURL"],
                                ReceiptHandle = sqsMessage.ReceiptHandle
                            });

                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError(ex, "Error while consuming message.");
                        }
                    }

                }
            }
        }

        private Task<ReceiveMessageResponse> GetMessagesFromSQS(CancellationToken stoppingToken)
        {
            var queueUrl = _configuration["AmazonSQSCheckoutQueueURL"];

            _logger.LogInformation("Trying to get new messages from {queueUrl}", queueUrl);

            var request = new ReceiveMessageRequest()
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 10,
                VisibilityTimeout = 5 * 60,
                WaitTimeSeconds = 15
            };

            return _amazonSQSClient.ReceiveMessageAsync(request, stoppingToken);
        }

        private async Task ConsumeMessage(Message sqsMessage, IScope scope)
        {
            _logger.LogInformation("Received message: {@message}", sqsMessage);

            var startCheckoutMessage = JsonConvert.DeserializeObject<StartCheckoutQueueMessage>(sqsMessage.Body);

            scope.Span.SetBaggageItem("cartId", startCheckoutMessage.Id);
            scope.Span.SetBaggageItem("controlId", startCheckoutMessage.ControlId);
            
            var scopeState = new Dictionary<string, string>()
            {
                { "cartId", startCheckoutMessage.Id },
                { "controlId", startCheckoutMessage.ControlId }
            };
            using (var loggerScope = _logger.BeginScope(scopeState))
            {
                var sendToInvoiceResponse = await _mediator.Send(new SendToInvoiceRequest()
                {
                    StartCheckoutMessage = startCheckoutMessage
                });

                var publishToTimelineResponse = await _mediator.Send(new PublishToTimelineRequest()
                {
                    InvoiceRequest = sendToInvoiceResponse.InvoiceRequest,
                    StartCheckoutMessage = startCheckoutMessage
                });
            }
        }
      
    }
}
