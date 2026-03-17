using Ai_ShopBot.Application.Extensions;
using Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById;
using Ai_ShopBot.Application.Features.Orders.Queries.GetOrdersWithPagination;
using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Croe.DTOs;
using Ai_ShopBot.Croe.Models;
using Ai_ShopBot.Presistance.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Ai_ShopBot.Presistance.Repos
{
    internal class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ShopDbContext _context;

        public OrderRepository(ShopDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<GetOrdersWithPaginationQueryDto>> GetOrderWithPagination<T>(
            string userId,
            int pageNumber,
            int PageSize,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(x => x.ClientId == userId)
                .ProjectToType<GetOrdersWithPaginationQueryDto>()
                .ToPaginatedListAsync(pageNumber, PageSize, cancellationToken);
        }

        public async Task<List<GetOrderByIdQueryDto>> GetOrderItemsByOrderId(int id,string userId)
        {
            return await _context.OrderItems
                .Where(x => x.OrderId == id)
                .ProjectToType<GetOrderByIdQueryDto>()
                .ToListAsync();
        }
    }
}