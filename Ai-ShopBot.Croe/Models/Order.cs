namespace Ai_ShopBot.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public virtual User Client { get; set; }
        public string Address { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
