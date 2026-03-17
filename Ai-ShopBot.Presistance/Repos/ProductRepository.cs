using Ai_ShopBot.Croe.Interfaces.Repo;
using Ai_ShopBot.Croe.Models;
using MongoDB.Driver;

namespace Ai_ShopBot.Presistance.Repos
{
    internal class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;
        public ProductRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("ShopBot");
            _productCollection = database.GetCollection<Product>("products");
        }

        public IMongoCollection<Product> Entities => _productCollection!;

        public override async Task AddAsync(Product entity)
        {
            await _productCollection.InsertOneAsync(entity);
        }

        public override async Task AddRange(IEnumerable<Product> entities)
        {
            await _productCollection.InsertManyAsync(entities);
        }

        public override async void Delete(Product entity)
        {
            await _productCollection.DeleteOneAsync(p => p.Id == entity.Id);
        }

        public override async void Update(Product entity)
        {
            await _productCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }

        public override async void UpdateRange(IEnumerable<Product> entities)
        {
            foreach (var entity in entities)
            {
                await _productCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
            }
        }

    }
}
