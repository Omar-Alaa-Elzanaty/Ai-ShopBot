using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Croe.DTOs;
using Ai_ShopBot.Croe.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Ai_ShopBot.Application.Features.Auth.Login
{
    public class LoginQuery : IRequest<BaseResponse<LoginQueryDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    internal class LoginQueryHandler : IRequestHandler<LoginQuery, BaseResponse<LoginQueryDto>>
    {
        private readonly SignInManager<User> _signManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthServices _authServices;

        public LoginQueryHandler(
            SignInManager<User> signManager,
            UserManager<User> userManager,
            IAuthServices authServices)
        {
            _signManager = signManager;
            _userManager = userManager;
            _authServices = authServices;
        }

        public async Task<BaseResponse<LoginQueryDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return BaseResponse<LoginQueryDto>.Failure("Invalid username or password", HttpStatusCode.Unauthorized);
            }

            var isValid = await _signManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!isValid.Succeeded)
            {
                return BaseResponse<LoginQueryDto>.Failure("Invalid username or password", HttpStatusCode.Unauthorized);
            }

            return BaseResponse<LoginQueryDto>.Success(new LoginQueryDto
            {
                FullName = user.FullName,
                Token = await _authServices.GenerateTokenAsync(user)
            });
        }
    }
}
