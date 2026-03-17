using Ai_ShopBot.Croe.DTOs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ai_ShopBot.Application.Features.Orders.Commands.Create
{
    public class CreateOrderPlugin
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateOrderPlugin(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public class ProductItemDto
        {
            [Required]
            [BsonRepresentation(BsonType.ObjectId)]
            [Description("The Id of the product to be ordered. If user didn't provide it try to get it depend on last chat history.")]
            public string ProductId { get; set; }
            [Required]
            [Description("The quantity of the product to be ordered. Must be a positive integer.")]
            public int Quantity { get; set; }
        }

        [KernelFunction]
        [Description("""  
            - Create a new order based on the user's request.
            - Return the [Data] ATTRIBUTE that comes from return value if successful, explain it as order Id.
            - In case you couldn't get any of required order attribute inform user to input or declare it except ProductId try to get it from last chat result.
            - In case [IsSuccess] ATTRIBUTE is false return the [Message] ATTRIBUTE that comes from return value to inform user about the failure reason.
            """)]
        public async Task<BaseResponse<int>> CreateOrder(
            [Required]
            [Description("User address where order will sent.")]
        string address,
            [Required]
            [Description("Product to be ordered. Quantity default value is 1.,item must not be empty or null.")]
            ProductItemDto item)
        {
            var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(new CreateOrderCommand()
            {
                Address = address,
                Items = [new CreateOrderItemDtos() { ProductId = item.ProductId, Quantity = item.Quantity }]
            });
        }
    }

}