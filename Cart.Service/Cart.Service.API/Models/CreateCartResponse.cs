using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cart.Service.API.Models
{
    public class CartResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("items")]
        public IEnumerable<CartItem> Items { get; set; }

        public class CartItem
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }

            [JsonProperty("scale")]
            public int Scale { get; set; }

            [JsonProperty("currencyCode")]
            public string CurrencyCode { get; set; }
        }
    }


}