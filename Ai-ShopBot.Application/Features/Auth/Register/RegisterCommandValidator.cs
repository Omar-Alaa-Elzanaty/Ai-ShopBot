using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Application.Features.Auth.Register
{
    public class RegisterCommandValidator:AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            
        }
    }
}
