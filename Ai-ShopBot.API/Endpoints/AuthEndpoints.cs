using Ai_ShopBot.Application.Features.Auth.Login;
using Ai_ShopBot.Application.Features.Auth.Register;
using Carter;
using MediatR;

namespace Ai_ShopBot.API.Endpoints
{
    public class AuthEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Auth");
            group.MapPost("Register", Register);
            group.MapPost("Login", Login);
        }
        public async Task<IResult> Register(RegisterCommand command, ISender sender)
        {
            return Results.Ok(await sender.Send(command));
        }

        public async Task<IResult> Login(LoginQuery query, ISender sender)
        {
            return Results.Ok(await sender.Send(query));
        }
    }
}
