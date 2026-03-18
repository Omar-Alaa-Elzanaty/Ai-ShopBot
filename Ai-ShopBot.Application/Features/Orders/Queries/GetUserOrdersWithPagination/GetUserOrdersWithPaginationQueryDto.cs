using Ai_ShopBot.Core.Enums;

namespace Ai_ShopBot.Application.Features.Orders.Queries.GetUserOrdersWithPagination
{
    public class GetUserOrdersWithPaginationQueryDto
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
    }
}
