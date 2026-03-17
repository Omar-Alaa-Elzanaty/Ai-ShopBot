using Ai_ShopBot.Croe.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using MongoDB.Driver;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;

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
