using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Service.Worker.Models;
using Checkout.Service.Worker.Services;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Checkout.Service.Worker.Timeline
{
    public class PublishToTimelineHandler : IRequestHandler<PublishToTimelineRequest, PublishToTimelineResponse>
    {
        private readonly ILogger<PublishToTimelineHandler> _logger;
        private readonly KafkaTimelineProducer _kafkaTimelineProducer;

        public PublishToTimelineHandler(ILogger<PublishToTimelineHandler> logger, KafkaTimelineProducer kafkaTimelineProducer)
        {
            _logger = logger;
            _kafkaTimelineProducer = kafkaTimelineProducer;

        }
        public async Task<PublishToTimelineResponse> Handle(PublishToTimelineRequest request, CancellationToken cancellationToken)
        {
            var timelineEvent = new TimelineOrderEvent()
            {
                Headers = new TimelineOrderEvent.TimelineOrderEventHeaders()
                {
                    ControlId = request.StartCheckoutMessage.ControlId
                },
                Payload = new TimelineOrderEvent.TimelineOrderEventPayload()
                {
                    CartId = request.StartCheckoutMessage.Id,
                    Price = new TimelineOrderEvent.TimelineOrderEventPayload.TimelineOrderEventPayloadPrice()
                    {
                        Amount = request.InvoiceRequest.Total.Amount,
                        CurrencyCode = request.InvoiceRequest.Total.CurrencyCode,
                        Scale = request.InvoiceRequest.Total.Scale
                    }
                }
            };

            _logger.LogInformation("Timeline order event: {@event}", timelineEvent);

            var message = new Message<Null, string>()
            {
                Value = JsonConvert.SerializeObject(timelineEvent)
            };

            await _kafkaTimelineProducer.PublishAsync(message);

            return new PublishToTimelineResponse();
        }
    }
}