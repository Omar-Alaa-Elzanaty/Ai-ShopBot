using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Core.Models;
using StackExchange.Redis;

namespace Ai_ShopBot.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository ProductsRepo { get; }
        IOrderRepository OrdersRepo { get; }
        IBaseRepository<Cart> CartsRepo { get; }
        IDatabase Redis { get; }
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
