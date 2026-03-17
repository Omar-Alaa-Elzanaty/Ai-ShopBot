using Ai_ShopBot.Croe.Interfaces;
using Ai_ShopBot.Croe.Interfaces.Repo;
using Ai_ShopBot.Croe.Models;
using Ai_ShopBot.Presistance.Context;
using StackExchange.Redis;

namespace Ai_ShopBot.Presistance
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ShopDbContext _shopDbContext;
        private readonly IDatabase _redis;

        public UnitOfWork(
            ShopDbContext shopDbContext,
            IProductRepository productsRepo,
            IBaseRepository<Cart> cartsRepo,
            IOrderRepository ordersRepo,
            IConnectionMultiplexer redis)
        {
            _shopDbContext = shopDbContext;
            ProductsRepo = productsRepo;
            CartsRepo = cartsRepo;
            OrdersRepo = ordersRepo;
            _redis= redis.GetDatabase();
        }

        public IProductRepository ProductsRepo { get; private set; }

        public IOrderRepository OrdersRepo { get; private set; }

        public IBaseRepository<Cart> CartsRepo { get; private set; }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _shopDbContext.SaveChangesAsync(cancellationToken);
        }

        public IDatabase Redis => _redis;
    }
}
