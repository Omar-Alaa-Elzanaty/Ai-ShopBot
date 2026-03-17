using Ai_ShopBot.Croe.DTOs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Ai_ShopBot.Application.Features.Products.Queries.GetProductsWithPrompt.GetProductsWithPrompt
{
    public class GetProductsWithPromptPlugin
    {
        private readonly IServiceProvider _serviceProvider;

        public GetProductsWithPromptPlugin(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [KernelFunction]
        [Description("Get products from the store inventory based on what the user is looking for." +
            "Return all data that function return in good format like each product represent a card.")]
        public async Task<BaseResponse<List<GetProductsWithPromptQueryDto>>> GetProducts(
            [Description("EXACT words the user typed. Do NOT rephrase, translate, or modify. Pass as-is.")]
            string prompt,
            [Description("Number of products to return. Default is 10.")]
            int limit = 10)
        {
            var scope= _serviceProvider.CreateScope();
            var mediator= scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(new GetProductsWithPromptQuery
            {
                Prompt = prompt,
                Limit = limit
            });
        }
    }
}
