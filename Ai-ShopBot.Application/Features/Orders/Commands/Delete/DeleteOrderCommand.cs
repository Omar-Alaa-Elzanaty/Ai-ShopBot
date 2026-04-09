using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.DTOs;
using FluentValidation;
using MediatR;
using System.Net;

namespace Ai_ShopBot.Application.Features.Orders.Commands.Delete
{
    public class DeleteOrderCommand : IRequest<BaseResponse<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, BaseResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteOrderCommand> _validator;

        public DeleteOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<DeleteOrderCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<BaseResponse<int>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseResponse<int>.ValidationFailure(validationResult.Errors);
            }

            var order = await _unitOfWork.OrdersRepo.GetByIdAsync(request.Id, cancellationToken);

            if (order is null)
                return BaseResponse<int>.Failure("Order not found", HttpStatusCode.NotFound);


            _unitOfWork.OrdersRepo.Delete(order);
            await _unitOfWork.SaveAsync(cancellationToken);

            return BaseResponse<int>.Success(request.Id, "Order deleted successfully");
        }
    }
}
