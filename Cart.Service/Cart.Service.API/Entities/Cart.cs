using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cart.Service.API.Entities
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("items")]
        public IEnumerable<CartItem> Items { get; set; }
    }
}