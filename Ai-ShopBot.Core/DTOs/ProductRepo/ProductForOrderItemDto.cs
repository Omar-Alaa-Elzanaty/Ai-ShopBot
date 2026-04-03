using Ai_ShopBot.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ai_ShopBot.Core.DTOs.ProductRepo
{
    public class ProductForOrderItemDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public ProductSizes Size { get; set; }
        public string Color { get; set; }
    }
}
