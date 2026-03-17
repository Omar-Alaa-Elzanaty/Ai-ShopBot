using Ai_ShopBot.Application.Features.Orders.Queries.GetOrdersWithPagination;
using Ai_ShopBot.Croe.DTOs;
using Ai_ShopBot.Croe.Models;

namespace Ai_ShopBot.Application.Interfaces.Repo
{
    public interface IOrderRepository:IBaseRepository<Order>
    {
        Task<PaginatedResponse<GetOrdersWithPaginationQueryDto>> GetOrderWithPagination<T>(
            string userId,
            int pageNumber,
            int PageSize,
            CancellationToken cancellationToken);
        Task<List<T>> GetOrderItemsByOrderId<T>(int id,string userId);
    }
}
