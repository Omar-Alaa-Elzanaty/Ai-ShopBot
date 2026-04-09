using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Application.Features.Orders.Commands.Delete
{
    internal class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
