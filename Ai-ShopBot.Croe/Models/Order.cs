namespace Ai_ShopBot.Croe.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual User Client { get; set; }
        public string Address { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
