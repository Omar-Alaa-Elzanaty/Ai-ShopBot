using Ai_ShopBot.Application.Features.Orders.Commands.Delete;
using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.Constants;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ai_ShopBot.API.Endpoints
{
    public class ClientEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Client")
                .RequireAuthorization(policy => policy.RequireRole(Roles.Client));

            group.MapGet("AiChat", AiChat);
            group.MapDelete("Orders/{orderId}", DeleteOrder);
        }

        public async Task AiChat(
            [FromQuery] string prompt,
            IAiChatAssistantServices aiChatAssistantServices,
            HttpResponse response,
            CancellationToken cancellationToken)
        {
            response.ContentType = "text/plain";

            await foreach (var chunck in aiChatAssistantServices.AIChattingAsync(prompt, cancellationToken))
            {
                await response.WriteAsync(chunck, cancellationToken: cancellationToken);
                await response.Body.FlushAsync(cancellationToken);
            }
        }

        public async Task<IResult> DeleteOrder(
            int orderId,
            ISender sender)
        {
            var result = await sender.Send(new DeleteOrderCommand { Id = orderId });
            return Results.Ok(result);
        }
    }
}
