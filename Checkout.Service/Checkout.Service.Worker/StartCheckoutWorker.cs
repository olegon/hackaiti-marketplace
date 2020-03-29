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

namespace Checkout.Service.Worker
{
    public class StartCheckoutWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StartCheckoutWorker> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly AmazonSQSClient _amazonSQSClient;
        private readonly IHttpInvoiceService _invoiceService;

        public StartCheckoutWorker(
            IConfiguration configuration,
            ILogger<StartCheckoutWorker> logger,
            IMediator mediator,
            IMapper mapper,
            AmazonSQSClient amazonSQSClient,
            IHttpInvoiceService invoiceService)
        {
            _configuration = configuration;
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _amazonSQSClient = amazonSQSClient;
            _invoiceService = invoiceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var receiveMessageResponse = await GetMessagesFromSQS(stoppingToken);

                foreach (var sqsMessage in receiveMessageResponse.Messages)
                {
                    using (var ctx = _logger.BeginScope(new Dictionary<string, string>() { { "ReceiptHandle", sqsMessage.ReceiptHandle } }))
                    {
                        try
                        {
                            await ConsumeMessage(sqsMessage);

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

        private async Task ConsumeMessage(Message sqsMessage)
        {
            _logger.LogInformation("Received message: {@message}", sqsMessage);

            var startCheckoutMessage = JsonConvert.DeserializeObject<StartCheckoutQueueMessage>(sqsMessage.Body);

            var scopeState = new Dictionary<string, string>()
            {
                { "cartId", startCheckoutMessage.Id },
                { "controlId", startCheckoutMessage.ControlId }
            };
            using (var scope = _logger.BeginScope(scopeState))
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
