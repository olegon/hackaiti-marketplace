using MongoDB.Bson.Serialization.Attributes;

namespace Cart.Service.API.Entities
{
    public class ProductPrice
    {
        [BsonElement("amount")]
        public int Amount { get; set; }

        [BsonElement("scale")]
        public int Scale { get; set; }

        [BsonElement("currencyCode")]
        public string CurrencyCode { get; set; }
    }
}
