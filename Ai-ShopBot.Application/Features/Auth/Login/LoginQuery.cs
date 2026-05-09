using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.DTOs;
using Ai_ShopBot.Core.Models;
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
        private readonly IUnitOfWork _unitOfWork;

        public LoginQueryHandler(
            SignInManager<User> signManager,
            UserManager<User> userManager,
            IAuthServices authServices,
            IUnitOfWork unitOfWork)
        {
            _signManager = signManager;
            _userManager = userManager;
            _authServices = authServices;
            _unitOfWork = unitOfWork;
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

            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = _authServices.GenerateRefreshToken(),
                ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.RefreshTokenRepo.AddAsync(refreshToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return BaseResponse<LoginQueryDto>.Success(new LoginQueryDto
            {
                FullName = user.FullName,
                Token = await _authServices.GenerateTokenAsync(user),
                RefreshToken = refreshToken.Token
            });
        }
    }
}
