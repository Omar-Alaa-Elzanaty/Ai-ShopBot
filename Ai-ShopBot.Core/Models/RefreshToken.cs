namespace Ai_ShopBot.Core.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
