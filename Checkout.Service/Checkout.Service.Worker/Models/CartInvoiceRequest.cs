using System.Collections.Generic;
using Newtonsoft.Json;

namespace Checkout.Service.Worker.Models
{
    public class CartInvoiceRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("total")]
        public CartTotal Total { get; set; }

        [JsonProperty("items")]
        public IEnumerable<CartItem> Items { get; set; }

        public class CartTotal
        {
            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("scale")]
            public int Scale { get; set; }

            [JsonProperty("currencyCode")]
            public string CurrencyCode { get; set; }
        }

        public class CartItem
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("imageUrl")]
            public string ImageURL { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }

            [JsonProperty("scale")]
            public int Scale { get; set; }

            [JsonProperty("currencyCode")]
            public string CurrencyCode { get; set; }
        }
    }
}