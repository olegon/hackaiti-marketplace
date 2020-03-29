using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Checkout.Service.Worker.Services
{
    public class KafkaTimelineProducer : IDisposable
    {
        private readonly ILogger<KafkaTimelineProducer> _logger;
        private ProducerConfig _producerConfig;
        private string _topicName;
        private IProducer<Null, string> _producer;

        public KafkaTimelineProducer(ILogger<KafkaTimelineProducer> logger, IConfiguration configuration)
        {
            _logger = logger;

            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = configuration["KafkaBootstrapServers"]
            };

            _topicName = configuration["KafkaTimelineOrderTopicName"];

            _logger.LogInformation("Creating Kafka Producer: {@producerConfig}", _producerConfig);

            _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        }

        public virtual async Task<DeliveryResult<Null, string>> PublishAsync(Message<Null, string> message)
        {
            _logger.LogInformation("Producing Kafka message: {@message}", message);

            var result = await _producer.ProduceAsync(_topicName, message);

            _logger.LogInformation("Producing Kafka message result: {@result}", result);

            return result;
        }
        public virtual void Dispose()
        {
            _logger.LogInformation("Disposing Kafka Producer: {@producerConfig}", _producerConfig);

            _producer.Dispose();
        }
    }
}