using Ai_ShopBot.Croe.Models;
using MongoDB.Driver;

namespace Ai_ShopBot.Croe.Interfaces.Repo
{
    public interface IProductRepository
    {
        IMongoCollection<Product> Entities { get; }

        Task AddAsync(Product entity);

        Task AddRange(IEnumerable<Product> entities);

        void Delete(Product entity);

        void Update(Product entity);

        void UpdateRange(IEnumerable<Product> entities);
    }
}
