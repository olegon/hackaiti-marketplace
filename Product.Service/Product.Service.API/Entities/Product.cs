using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Product.Service.API.Entities
{
    public class Product
    {
        [BsonId()]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("sku")]
        public string SKU { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("shortDescription")]
        public string ShortDescription { get; set; }

        [BsonElement("longDescription")]
        public string LongDescription { get; set; }

        [BsonElement("imageURL")]
        public string ImageURL { get; set; }

        [BsonElement("price")]
        public ProductPrice Price { get; set; }
    }
}