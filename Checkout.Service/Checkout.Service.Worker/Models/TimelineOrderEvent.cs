using Newtonsoft.Json;

namespace Checkout.Service.Worker.Models
{
    public class TimelineOrderEvent
    {
        [JsonProperty("headers")]
        public TimelineOrderEventHeaders Headers { get; set; }

        [JsonProperty("payload")]
        public TimelineOrderEventPayload Payload { get; set; }

        public class TimelineOrderEventHeaders
        {
            [JsonProperty("x-team-control")]
            public string ControlId { get; set; }
        }

        public class TimelineOrderEventPayload
        {
            [JsonProperty("cartId")]
            public string CartId { get; set; }

            [JsonProperty("price")]
            public TimelineOrderEventPayloadPrice Price { get; set; }

            public class TimelineOrderEventPayloadPrice
            {
                [JsonProperty("amount")]
                public int Amount { get; set; }

                [JsonProperty("scale")]
                public int Scale { get; set; }

                [JsonProperty("currencyCode")]
                public string CurrencyCode { get; set; }
            }
        }
    }
}
