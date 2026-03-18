using Ai_ShopBot.Application.Features.Admin.Queries;
using Ai_ShopBot.Application.Features.Products.Commands.CreateProduct;
using Ai_ShopBot.Core.Constants;
using Ai_ShopBot.Core.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ai_ShopBot.API.Controllers
{
    //[Authorize(Roles = Roles.Admin)]
    public class AdminController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Products")]
        public async Task<ActionResult<BaseResponse<string>>> CreateProducts(List<CreateProductCommand> command)
        {
            foreach (var item in command)
            {
                await _mediator.Send(item);
            }

            return Ok();
        }

        [HttpGet("Orders")]
        public async Task<ActionResult<PaginatedResponse<GetOrdersWithPaginationQuery>>> GetOrders([FromQuery] GetOrdersWithPaginationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
