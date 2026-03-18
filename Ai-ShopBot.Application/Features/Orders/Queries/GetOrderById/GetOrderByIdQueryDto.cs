using Ai_ShopBot.Core.Enums;

namespace Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public ProductSizes Size { get; set; }
        public string Color { get; set; }
    }
}
