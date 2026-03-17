using Ai_ShopBot.Croe.Models;

namespace Ai_ShopBot.Application.Features.Admin.Queries
{
    public class GetOrdersWithPaginationQueryDto
    {
        public string ClientId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
    }
}
