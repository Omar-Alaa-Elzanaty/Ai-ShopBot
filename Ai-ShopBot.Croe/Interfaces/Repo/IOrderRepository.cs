using Ai_ShopBot.Croe.DTOs;

namespace Ai_ShopBot.Croe.Interfaces.Repo
{
    public interface IOrderRepository
    {
        Task<PaginatedResponse<T>> GetOrderWithPagination<T>(string userId,int pageNumber, int PageSize, CancellationToken cancellationToken);
    }
}
