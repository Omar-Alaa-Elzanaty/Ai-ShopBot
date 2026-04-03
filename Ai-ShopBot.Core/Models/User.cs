using Microsoft.AspNetCore.Identity;

namespace Ai_ShopBot.Core.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
