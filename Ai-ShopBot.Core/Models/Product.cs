using Ai_ShopBot.Core.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ai_ShopBot.Core.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public ProductSizes Size { get; set; }
        public string Description { get; set; }
        public string ImageUrl {  get; set; }
        [BsonElement("plot_embedding_1024")]
        public float[] PlotEmbedding1024 { get; set; }
    }
}
