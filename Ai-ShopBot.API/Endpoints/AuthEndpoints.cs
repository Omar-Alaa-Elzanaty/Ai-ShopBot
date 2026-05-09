using Ai_ShopBot.Application.Features.Auth.Login;
using Ai_ShopBot.Application.Features.Auth.LoginWithRefreshToken;
using Ai_ShopBot.Application.Features.Auth.Register;
using Ai_ShopBot.Application.Features.Auth.RevokeRefreshtoken;
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
            group.MapPost("refresh-token", LoginWithRefreshToken);
            group.MapDelete("users/refresh-tokens", RevokeRefreshToken);
        }

        public async Task<IResult> Register(RegisterCommand command, ISender sender)
        {
            return Results.Ok(await sender.Send(command));
        }

        public async Task<IResult> Login(LoginQuery query, ISender sender)
        {
            return Results.Ok(await sender.Send(query));
        }

        public async Task<IResult> LoginWithRefreshToken(LoginWithRefreshTokenQuery query, ISender sender)
        {
            return Results.Ok(await sender.Send(query));
        }

        public async Task<IResult> RevokeRefreshToken(ISender sender)
        {
            return Results.Ok(await sender.Send(new RevokeRefreshtoken()));
        }
    }
}
