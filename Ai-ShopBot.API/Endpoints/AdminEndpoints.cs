using Ai_ShopBot.Application.Features.Admin.Queries;
using Ai_ShopBot.Application.Features.Products.Commands.CreateProduct;
using Ai_ShopBot.Core.Constants;
using Carter;
using MediatR;

namespace Ai_ShopBot.API.Endpoints
{
    public class AdminEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Admin")
                .RequireAuthorization(policy => policy.RequireRole(Roles.Admin));

            group.MapPost("Products", CreateProducts);
            group.MapGet("Orders", GetOrders);
        }

        public async Task<IResult> CreateProducts(
            List<CreateProductCommand> command,
            ISender sender)
        {
            foreach (var item in command)
            {
                await sender.Send(item);
            }

            return Results.Ok();
        }

        public async Task<IResult> GetOrders(
            [AsParameters] GetOrdersWithPaginationQuery query,
            ISender sender)
        {
            return Results.Ok(await sender.Send(query));
        }
    }
}
