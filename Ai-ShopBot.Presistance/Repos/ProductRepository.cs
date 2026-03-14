using Ai_ShopBot.Croe.Interfaces.Repo;
using Ai_ShopBot.Croe.Models;
using Ai_ShopBot.Presistance.Context;

namespace Ai_ShopBot.Presistance.Repos
{
    internal class ProductRepository :BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopDbContext shopDbContext)
        {

        }

        public override IQueryable<Product> Entities => null!;

        public override async Task AddAsync(Product entity)
        {
            throw new NullReferenceException();
        }

        public override Task AddRange(IEnumerable<Product> entities)
        {
            return base.AddRange(entities);
        }

        public override void Delete(Product entity)
        {
            base.Delete(entity);
        }

        public override void Update(Product entity)
        {
            base.Update(entity);
        }

        public override void UpdateRange(IEnumerable<Product> entities)
        {
            base.UpdateRange(entities);
        }

    }
}
