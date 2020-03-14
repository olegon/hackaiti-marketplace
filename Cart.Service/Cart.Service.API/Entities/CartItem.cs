using MongoDB.Bson.Serialization.Attributes;

namespace Cart.Service.API.Entities
{
    public class CartItem
    {
        [BsonElement("price")]
        public int Price { get; set; }

        [BsonElement("scale")]
        public int Scale { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("product")]
        public Product Product { get; set; }
    }
}