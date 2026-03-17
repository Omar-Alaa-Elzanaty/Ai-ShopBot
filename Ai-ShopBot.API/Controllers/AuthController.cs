using Ai_ShopBot.Application.Features.Auth.Login;
using Ai_ShopBot.Application.Features.Auth.Register;
using Ai_ShopBot.Croe.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ai_ShopBot.API.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<BaseResponse<string>>> Register(RegisterCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse<LoginQueryDto>>> Login(LoginQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
