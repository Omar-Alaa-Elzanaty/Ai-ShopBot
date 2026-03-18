using FluentValidation;
using MongoDB.Bson;

namespace Ai_ShopBot.Application.Features.Orders.Commands.Create
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {

        }
    }
}
