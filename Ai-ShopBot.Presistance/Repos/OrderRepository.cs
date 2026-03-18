using Ai_ShopBot.Application.Extensions;
using Ai_ShopBot.Application.Features.Admin.Queries;
using Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById;
using Ai_ShopBot.Application.Features.Orders.Queries.GetUserOrdersWithPagination;
using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Core.DTOs;
using Ai_ShopBot.Core.Models;
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

        public async Task<PaginatedResponse<GetUserOrdersWithPaginationQueryDto>> GetOrderWithPagination<T>(
            string userId,
            int pageNumber,
            int PageSize,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(x => x.ClientId == userId)
                .ProjectToType<GetUserOrdersWithPaginationQueryDto>()
                .ToPaginatedListAsync(pageNumber, PageSize, cancellationToken);
        }

        public async Task<List<GetOrderByIdQueryDto>> GetUserOrderItemsByOrderId(int id, string userId)
        {
            return await _context.OrderItems
                .Where(x => x.OrderId == id)
                .ProjectToType<GetOrderByIdQueryDto>()
                .ToListAsync();
        }

        public async Task<PaginatedResponse<GetOrdersWithPaginationQueryDto>> GetOrdersWithPaginationToAdmin(int pageNumber, int pageSize)
        {
            var config = new TypeAdapterConfig();

            config.NewConfig<Order, GetOrdersWithPaginationQueryDto>()
                .Map(dest => dest.FullName, src => src.Client.FullName);

            return await _context.Orders
                .OrderByDescending(o => o.Date)
                .ProjectToType<GetOrdersWithPaginationQueryDto>(config)
                .ToPaginatedListAsync(pageNumber, pageSize, CancellationToken.None);
        }
    }
}