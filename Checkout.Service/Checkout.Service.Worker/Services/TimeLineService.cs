using System;
using System.Threading.Tasks;
using Checkout.Service.Worker.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Checkout.Service.Worker.Services
{
    public class TimeLineService : ITimelineService, IDisposable
    {
        private readonly ILogger<TimeLineService> _logger;
        private ProducerConfig _producerConfig;
        private string _topicName;
        private IProducer<Null, string> _producer;

        public TimeLineService(ILogger<TimeLineService> logger, IConfiguration configuration)
        {
            _logger = logger;

            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = configuration["KafkaBootstrapServers"]
            };

            _topicName = configuration["KafkaTimelineOrderTopicName"];

            _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        }

        public void Dispose()
        {
            _producer.Dispose();
        }

        public async Task PublishTimelineOrderEvent(TimelineOrderEvent timelineEvent)
        {
            _logger.LogInformation("Timeline order event: {@event}", timelineEvent);

            var message = new Message<Null, string>()
            {
                Value = JsonConvert.SerializeObject(timelineEvent)
            };

            var result = await _producer.ProduceAsync(_topicName, message);

            _logger.LogInformation("Kafka puslih result: {@result}", result);
        }
    }
}