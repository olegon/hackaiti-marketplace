using Newtonsoft.Json;

namespace Product.Service.API.Model
{
    public class ProductResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("longDescription")]
        public string LongDescription { get; set; }

        [JsonProperty("imageURL")]
        public string ImageURL { get; set; }

        [JsonProperty("price")]
        public ProductPrice Price { get; set; }

        public class ProductPrice
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