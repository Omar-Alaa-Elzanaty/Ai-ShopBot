using Ai_ShopBot.Croe.DTOs;
using Ai_ShopBot.Croe.Interfaces;
using Ai_ShopBot.Croe.Models;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Ai_ShopBot.Application.Features.Orders.Commands.Create
{
    public class CreateOrderCommand : IRequest<BaseResponse<int>>
    {
        public string Address { get; set; }
        public List<CreateOrderItemDtos> Items { get; set; }
    }

    public class CreateOrderItemDtos
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IValidator<CreateOrderCommand> _validator;

        public CreateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<CreateOrderCommand> validator,
            IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContext = httpContext;
        }

        public async Task<BaseResponse<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseResponse<int>.ValidationFailure(validationResult.Errors);
            }

            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var order = request.Adapt<Order>();
            order.ClientId = userId.Value;

            await _unitOfWork.OrdersRepo.AddAsync(order);
            await _unitOfWork.SaveAsync(cancellationToken);

            return BaseResponse<int>.Success(order.Id, "Order created successfully", HttpStatusCode.Created);
        }
    }
}
