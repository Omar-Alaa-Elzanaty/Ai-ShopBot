using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.DTOs;
using Ai_ShopBot.Core.Models;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ai_ShopBot.Application.Features.Auth.Register
{
    public class RegisterCommand : IRequest<BaseResponse<string>>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseResponse<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthServices _authServices;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            IAuthServices authServices)
        {
            _userManager = userManager;
            _authServices = authServices;
        }

        public async Task<BaseResponse<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                return BaseResponse<string>.ValidationFailure(identityResult.Errors);
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            var token = await _authServices.GenerateTokenAsync(user);

            return BaseResponse<string>.Success(token, "User registered successfully");
        }
    }
}
