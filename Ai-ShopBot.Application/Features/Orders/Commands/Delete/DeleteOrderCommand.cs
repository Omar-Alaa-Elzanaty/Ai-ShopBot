using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.DTOs;
using Ai_ShopBot.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Ai_ShopBot.Application.Features.Orders.Commands.Delete
{
    public class DeleteOrderCommand : IRequest<BaseResponse<bool>>
    {
        public int Id { get; set; }
    }

    internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public DeleteOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var order = await _unitOfWork.OrdersRepo.GetByIdAsync(request.Id);
            if (order is null)
                return BaseResponse<bool>.Failure("Order not found", HttpStatusCode.NotFound);

            if (order.ClientId != userId.Value)
                return BaseResponse<bool>.Failure("You are not authorized to delete this order", HttpStatusCode.Forbidden);

            _unitOfWork.OrdersRepo.Delete(order);
            await _unitOfWork.SaveAsync(cancellationToken);

            return BaseResponse<bool>.Success(true, "Order deleted successfully");
        }
    }
}
