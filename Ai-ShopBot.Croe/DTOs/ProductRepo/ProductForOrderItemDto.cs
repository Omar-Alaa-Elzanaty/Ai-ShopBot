using Ai_ShopBot.Croe.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ai_ShopBot.Croe.DTOs.ProductRepo
{
    [BsonIgnoreExtraElements]
    public class ProductForOrderItemDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public ProductSizes Size { get; set; }
        public string Color { get; set; }
    }
}
