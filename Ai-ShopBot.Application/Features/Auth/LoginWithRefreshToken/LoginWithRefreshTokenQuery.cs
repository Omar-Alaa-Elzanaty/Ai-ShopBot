using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.DTOs;
using MediatR;

namespace Ai_ShopBot.Application.Features.Auth.LoginWithRefreshToken
{
    public class LoginWithRefreshTokenQuery : IRequest<BaseResponse<LoginWithRefreshTokenDto>>
    {
        public string RefreshToken { get; set; }
    }

    internal class LoginWithRefreshTokenQueryHandler : IRequestHandler<LoginWithRefreshTokenQuery, BaseResponse<LoginWithRefreshTokenDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthServices _authServices;

        public LoginWithRefreshTokenQueryHandler(
            IUnitOfWork unitOfWork,
            IAuthServices authServices)
        {
            _unitOfWork = unitOfWork;
            _authServices = authServices;
        }

        public async Task<BaseResponse<LoginWithRefreshTokenDto>> Handle(LoginWithRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var refreshToken = await _unitOfWork.RefreshTokenRepo.GetByTokenAsync(request.RefreshToken);

            if (refreshToken == null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                throw new ApplicationException("The refresh token is invalid or has expired.");
            }

            string accessToken = await _authServices.GenerateTokenAsync(refreshToken.User);

            refreshToken.Token = _authServices.GenerateRefreshToken();
            refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.SaveAsync(cancellationToken);

            return new BaseResponse<LoginWithRefreshTokenDto>()
            {
                Data = new LoginWithRefreshTokenDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                }
            };
        }
    }
}
