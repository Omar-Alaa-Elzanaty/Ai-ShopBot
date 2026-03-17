using Ai_ShopBot.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Ai_ShopBot.API.Hubs
{
    //[Authorize]
    public class ClientHub : Hub<IChatHub>
    {

        public ClientHub()
        {

        }
    }
}
