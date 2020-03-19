using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using AutoMapper;
using Checkout.Service.Worker.Models;
using Checkout.Service.Worker.Services;
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
        private readonly IMapper _mapper;
        private readonly AmazonSQSClient _amazonSQSClient;
        private readonly ICurrencyService _currencyService;
        private readonly IInvoiceService _invoiceService;

        public StartCheckoutWorker(
            IConfiguration configuration,
            ILogger<StartCheckoutWorker> logger,
            IMapper mapper,
            AmazonSQSClient amazonSQSClient,
            ICurrencyService currencyService,
            IInvoiceService invoiceService)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _amazonSQSClient = amazonSQSClient;
            _currencyService = currencyService;
            _invoiceService = invoiceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var receiveMessageResponse = await GeteMessagesFromSQS();

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

        private async Task ConsumeMessage(Message sqsMessage)
        {
            _logger.LogInformation("Received message: {@message}", sqsMessage);

            var checkout = JsonConvert.DeserializeObject<StartCheckoutQueueMessage>(sqsMessage.Body);

            var request = _mapper.Map<CartInvoiceRequest>(checkout);

            request.Total = await CalculateTotal(checkout);

            await _invoiceService.SendInvoice(request, checkout.ControlId);
        }

        private async Task<CartInvoiceRequest.CartTotal> CalculateTotal(StartCheckoutQueueMessage checkout)
        {
            var currencyTable = await _currencyService.GetCurrencies();

            var total = new CartInvoiceRequest.CartTotal()
            {
                Amount = 0,
                Scale = 2,
                CurrencyCode = checkout.CurrencyCode
            };

            foreach (var item in checkout.Items)
            {
                var factor = currencyTable.Factors[$"{item.CurrencyCode}_TO_{checkout.CurrencyCode}"];

                var currencPrice = item.Price / Math.Pow(10, item.Scale);
                var desidedPrice = currencPrice * factor * 100;

                total.Amount += (int)desidedPrice;
            }

            return total;
        }

        private Task<ReceiveMessageResponse> GeteMessagesFromSQS()
        {
            var queueUrl = _configuration["AmazonSQSCheckoutQueueURL"];

            _logger.LogInformation("Trying to get new messages from {queueUrl}", queueUrl);

            var request = new ReceiveMessageRequest()
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 1,
                VisibilityTimeout = 60,
                WaitTimeSeconds = 15
            };

            return _amazonSQSClient.ReceiveMessageAsync(request);
        }
    }
}
