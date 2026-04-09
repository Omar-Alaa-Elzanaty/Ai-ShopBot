using Ai_ShopBot.Application.Features.Admin.Queries;
using Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById;
using Ai_ShopBot.Application.Features.Orders.Queries.GetUserOrdersWithPagination;
using Ai_ShopBot.Core.DTOs;
using Ai_ShopBot.Core.Models;

namespace Ai_ShopBot.Application.Interfaces.Repo
{
    public interface IOrderRepository:IBaseRepository<Order>
    {
        Task<PaginatedResponse<GetUserOrdersWithPaginationQueryDto>> GetOrderWithPagination<T>(
            string userId,
            int pageNumber,
            int PageSize,
            CancellationToken cancellationToken);
        Task<List<GetOrderByIdQueryDto>> GetUserOrderItemsByOrderId(int id,string userId);
        Task<PaginatedResponse<GetOrdersWithPaginationQueryDto>> GetOrdersWithPaginationToAdmin(int pageNumber,int pageSize);

        Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken);

    }
}
