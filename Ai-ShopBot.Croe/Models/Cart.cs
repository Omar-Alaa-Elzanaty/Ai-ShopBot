using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Croe.Models
{
    public class Cart
    {
        public int ClientId { get; set; }
        public virtual User Client { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
