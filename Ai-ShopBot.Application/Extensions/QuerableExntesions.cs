using Ai_ShopBot.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Ai_ShopBot.Application.Extensions
{
    public static class QuerableExntesions
    {
        public static async Task<PaginatedResponse<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken,
        string? message = null)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await source.CountAsync(cancellationToken: cancellationToken);
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return await PaginatedResponse<T>.SuccessAsync(items, count, pageNumber, pageSize, message);
        }
    }
}
