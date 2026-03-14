using Ai_ShopBot.Croe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ai_ShopBot.Presistance.ModelsConfig
{
    internal class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(k => new { k.ClientId, k.ProductId });
        }
    }
}
