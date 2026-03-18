using FluentValidation;
using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Application.Features.Carts.Command.AddToCard
{
    public class AddToCardCommandValidator:AbstractValidator<AddToCardCommand>
    {
        public AddToCardCommandValidator()
        {
            
        }
    }
}
