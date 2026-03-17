using Ai_ShopBot.Croe.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public ProductSizes Size { get; set; }
        public string Color { get; set; }
    }
}
