using Ai_ShopBot.Croe.Enums;

namespace Ai_ShopBot.Croe.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public ProductSizes Size { get; set; }
        public string Description { get; set; }
        public List<string> ImagesUrl {  get; set; }
    }
}
