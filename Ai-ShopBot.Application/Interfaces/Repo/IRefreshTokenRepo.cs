using Ai_ShopBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Application.Interfaces.Repo
{
    public interface IRefreshTokenRepo:IBaseRepository<RefreshToken>
    {
        Task DeleteAllTokensByUserIdAsync(string? userId);
        Task<RefreshToken?> GetByTokenAsync(string token);
    }
}
