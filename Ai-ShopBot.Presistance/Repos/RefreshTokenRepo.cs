using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Core.Models;
using Ai_ShopBot.Presistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Ai_ShopBot.Presistance.Repos
{
    internal class RefreshTokenRepo : BaseRepository<RefreshToken>, IRefreshTokenRepo
    {
        private readonly ShopDbContext _context;

        public RefreshTokenRepo(ShopDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeleteAllTokensByUserIdAsync(string? userId)
        {
            await _context.RefreshTokens
                .Where(x => x.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token);
        }
    }
}
