using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Croe.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ai_ShopBot.API.Controllers
{
    [Authorize(Roles = Roles.Client)]
    public class ClientController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAiChatAssistantServices _aiChatAssistantServices;

        public ClientController(
            IAiChatAssistantServices aiChatAssistantServices,
            IMediator mediator)
        {
            _aiChatAssistantServices = aiChatAssistantServices;
            _mediator = mediator;
        }

        [HttpGet("AiChatting")]
        public async Task ChatWithAI([FromQuery] string prompt, CancellationToken cancellationToken)
        {
            Response.ContentType = "text/plain";

            await foreach (var response in _aiChatAssistantServices.AIChattingAsync(prompt, cancellationToken))
            {
                await Response.WriteAsync(response, cancellationToken: cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }
        }
    }
}
