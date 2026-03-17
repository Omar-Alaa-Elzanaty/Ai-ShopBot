using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            
        }
    }
}
