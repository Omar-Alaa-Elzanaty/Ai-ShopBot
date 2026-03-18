using Ai_ShopBot.Core.DTOs;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Globalization;

namespace Ai_ShopBot.Application.Features.Carts.Command.AddToCard
{
    public class AddToCardPlugin
    {
        private readonly IServiceProvider _serviceProvider;

        public AddToCardPlugin(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [KernelFunction]
        [Description("""
             - Add Product To cart
             - If [IsSuccess] Attribute true Return [Message] Attribute.
             - If [IsSuccess] Attribute false Return [Message] and [Errors] Attributes.
             - If User didn't provide product ID ,try to get ID of product from last Chat and then ask him to confirm add this product with this ID then Add It.
            """)]
        public async Task<BaseResponse<bool>> AddToCard(
            [Description("Product ID which data type is string in format Bson objectId")]
            string id,
            [Description("Quantity of Product,Default value 1")]
            int Qunatity)
        {
            var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(new AddToCardCommand()
            {
                ProductId = id,
                Quantity = Qunatity
            });
        }
    }
}
