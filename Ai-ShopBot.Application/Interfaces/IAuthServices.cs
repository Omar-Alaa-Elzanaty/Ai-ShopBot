using Ai_ShopBot.Croe.Models;

namespace Ai_ShopBot.Application.Interfaces
{
    public interface IAuthServices
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
