namespace Ai_ShopBot.Croe.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
