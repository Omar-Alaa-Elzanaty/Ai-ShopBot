using Ai_ShopBot.Core.Models;

namespace Ai_ShopBot.Application.Interfaces
{
    public interface IAuthServices
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
