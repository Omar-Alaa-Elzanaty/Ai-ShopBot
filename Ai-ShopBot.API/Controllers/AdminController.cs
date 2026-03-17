using Ai_ShopBot.Application.Features.Products.Commands.CreateProduct;
using Ai_ShopBot.Application.Features.Products.Queries.GetProductsWithPrompt;
using Ai_ShopBot.Croe.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ai_ShopBot.API.Controllers
{
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
    }
}
