using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Checkout.Service.Worker.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Checkout.Service.Worker
{
    public class StartCheckoutWorker : BackgroundService
    {
        private readonly ILogger<StartCheckoutWorker> _logger;
        private readonly AmazonSQSClient _amazonSQSClient;
        private readonly IConfiguration _configuration;

        public StartCheckoutWorker(IConfiguration configuration, ILogger<StartCheckoutWorker> logger, AmazonSQSClient amazonSQSClient)
        {
            _configuration = configuration;
            _logger = logger;
            _amazonSQSClient = amazonSQSClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var receiveMessageResponse = await GeteMessagesFromSQS();

                foreach (var sqsMessage in receiveMessageResponse.Messages)
                {
                    _logger.LogInformation("Received message: {@message}", sqsMessage);

                    var checkout = JsonConvert.DeserializeObject<StartCheckoutQueueMessage>(sqsMessage.Body);
                }
            }
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
                WaitTimeSeconds = 5
            };

            return _amazonSQSClient.ReceiveMessageAsync(request);
        }
    }
}
