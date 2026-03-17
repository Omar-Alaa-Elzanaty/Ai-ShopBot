using Ai_ShopBot.Croe.Interfaces.Repo;
using Ai_ShopBot.Croe.Models;
using StackExchange.Redis;

namespace Ai_ShopBot.Croe.Interfaces
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
