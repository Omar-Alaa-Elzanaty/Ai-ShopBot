using Microsoft.AspNetCore.Identity;

namespace Ai_ShopBot.Croe.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
