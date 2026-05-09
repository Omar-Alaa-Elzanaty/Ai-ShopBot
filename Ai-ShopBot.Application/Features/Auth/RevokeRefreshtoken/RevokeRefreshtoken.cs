using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Ai_ShopBot.Application.Features.Auth.RevokeRefreshtoken
{
    public sealed class RevokeRefreshtoken : IRequest<BaseResponse<bool>>;

    internal class RevokeRefreshtokenHandler : IRequestHandler<RevokeRefreshtoken, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RevokeRefreshtokenHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<bool>> Handle(RevokeRefreshtoken request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _unitOfWork.RefreshTokenRepo.DeleteAllTokensByUserIdAsync(userId);

            return BaseResponse<bool>.Success(true);
        }
    }
}
