using Ai_ShopBot.Core.DTOs;
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
        [Description("""
            
            - Get products from the store inventory based on what the user is looking for.
            - If [IsSuccess] Attribute is true return [Data] Attribute in form of cards to user,[ALWAYS] Return [Id] Attribute.
            - If [IsSuccess] Attribute is false return [Message] Attribute to declare problem to user.
            - ALWAYS RETURN  ID OF PRODUCT ATTRIBUTE WITH DATA.
            """)]
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
