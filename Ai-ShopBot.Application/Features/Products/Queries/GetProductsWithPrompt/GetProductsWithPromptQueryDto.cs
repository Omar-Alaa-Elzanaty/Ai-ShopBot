using Ai_ShopBot.Croe.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Application.Features.Products.Queries.GetProductsWithPrompt
{
    public class GetProductsWithPromptQueryDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public ProductSizes Size { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
