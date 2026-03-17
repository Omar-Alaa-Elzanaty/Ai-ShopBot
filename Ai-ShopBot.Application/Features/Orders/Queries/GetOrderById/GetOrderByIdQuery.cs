using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Croe.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById
{
    public sealed class GetOrderByIdQuery : IRequest<BaseResponse<List<GetOrderByIdQueryDto>>>
    {
        public int Id { get; set; }

        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
    }

    internal class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, BaseResponse<List<GetOrderByIdQueryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        private readonly IMapper _mapper;
        public GetOrderByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<GetOrderByIdQueryDto>>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var orderItems = await _unitOfWork.OrdersRepo
                .GetOrderItemsByOrderId<GetOrderByIdQueryDto>(request.Id, userId);

            if (orderItems == null || orderItems.Count == 0)
            {
                return BaseResponse<List<GetOrderByIdQueryDto>>.Failure("Order not found or you may have no access to this order.");
            }

            var products = await _unitOfWork.ProductsRepo
                .GetProductsForOrderItem(orderItems.Select(x => x.ProductId).ToList());

            foreach (var item in orderItems)
            {
                var product = products.First(x => x.Id == item.ProductId);
                _mapper.Map(product, item); //fetch product details
            }

            return BaseResponse<List<GetOrderByIdQueryDto>>.Success(orderItems);
        }
    }
}
