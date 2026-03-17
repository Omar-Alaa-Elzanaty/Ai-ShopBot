using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Croe.DTOs;
using MediatR;

namespace Ai_ShopBot.Application.Features.Admin.Queries
{
    public sealed record GetOrdersWithPaginationQuery:IRequest<PaginatedResponse<GetOrdersWithPaginationQueryDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    internal class GetOrdersWithPaginationQueryHandler : IRequestHandler<GetOrdersWithPaginationQuery, PaginatedResponse<GetOrdersWithPaginationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrdersWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResponse<GetOrdersWithPaginationQueryDto>> Handle(GetOrdersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.OrdersRepo
                .GetOrdersWithPaginationToAdmin(request.PageNumber,request.PageSize);
        }
    }
}
