using Ai_ShopBot.Application.Extensions;
using Ai_ShopBot.Croe.DTOs;
using Ai_ShopBot.Croe.Interfaces.Repo;
using Ai_ShopBot.Croe.Models;
using Mapster;

namespace Ai_ShopBot.Presistance.Repos
{
    internal class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public async Task<PaginatedResponse<T>> GetOrderWithPagination<T>(
            string userId,
            int pageNumber,
            int PageSize,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(x => x.ClientId == userId)
                .ProjectToType<T>()
                .ToPaginatedListAsync(pageNumber, PageSize, cancellationToken);
        }
    }
}
