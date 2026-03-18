using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ai_ShopBot.Application.Features.Orders.Queries.GetUserOrdersWithPagination
{
    public record GetUserOrdersWithPaginationQuery : IRequest<PaginatedResponse<GetUserOrdersWithPaginationQueryDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    internal class GetOrderWithPaginationQueryHandler : IRequestHandler<GetUserOrdersWithPaginationQuery, PaginatedResponse<GetUserOrdersWithPaginationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;

        public GetOrderWithPaginationQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<PaginatedResponse<GetUserOrdersWithPaginationQueryDto>> Handle(GetUserOrdersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var userId = _context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await _unitOfWork.OrdersRepo
                .GetOrderWithPagination<GetUserOrdersWithPaginationQueryDto>(userId, request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}