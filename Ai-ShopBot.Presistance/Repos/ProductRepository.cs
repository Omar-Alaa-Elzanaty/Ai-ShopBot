using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Core.DTOs.ProductRepo;
using Ai_ShopBot.Core.Models;
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

        public async Task<List<ProductForOrderItemDto>> GetProductsForOrderItem(List<string> productIds)
        {
            var filter = Builders<Product>.Filter.In(p => p.Id, productIds);

            var projection = Builders<Product>.Projection.Combine();

            return await _productCollection
                .Find(filter)
                .Project(p => new ProductForOrderItemDto
                {
                    Id = p.Id,
                    Color = p.Color,
                    Name = p.Name,
                    Size = p.Size
                })
                .ToListAsync();
        }
    }
}
