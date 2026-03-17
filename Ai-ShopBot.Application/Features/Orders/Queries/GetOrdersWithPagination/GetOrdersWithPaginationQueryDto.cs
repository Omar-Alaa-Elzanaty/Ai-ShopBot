using Ai_ShopBot.Croe.Enums;

namespace Ai_ShopBot.Application.Features.Orders.Queries.GetOrdersWithPagination
{
    public class GetOrdersWithPaginationQueryDto
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
    }
}
