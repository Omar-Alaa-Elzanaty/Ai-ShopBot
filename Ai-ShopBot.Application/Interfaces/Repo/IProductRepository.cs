using Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById;
using Ai_ShopBot.Croe.DTOs.ProductRepo;
using Ai_ShopBot.Croe.Models;
using MongoDB.Driver;

namespace Ai_ShopBot.Application.Interfaces.Repo
{
    public interface IProductRepository
    {
        IMongoCollection<Product> Entities { get; }

        Task AddAsync(Product entity);

        Task AddRange(IEnumerable<Product> entities);

        void Delete(Product entity);

        void Update(Product entity);

        void UpdateRange(IEnumerable<Product> entities);
        Task<List<ProductForOrderItemDto>> GetProductsForOrderItem(List<string> productIds);
    }
}
