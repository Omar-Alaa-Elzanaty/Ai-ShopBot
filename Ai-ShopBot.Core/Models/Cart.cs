namespace Ai_ShopBot.Core.Models
{
    public class Cart
    {
        public string ClientId { get; set; }
        public virtual User Client { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
