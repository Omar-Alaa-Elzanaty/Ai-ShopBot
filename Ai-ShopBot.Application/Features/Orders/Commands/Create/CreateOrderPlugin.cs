using Ai_ShopBot.Croe.DTOs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
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
            [Description("""
                The Id of the product to be ordered.
                [Id] data type MUST be bsonType.objectId with no white spaces.
                If you couldn't get it try to get from chat history or inform user to insert it.
                """)]
            public string ProductId { get; set; }
            [Required]
            [Description("The quantity of the product to be ordered. Must be a positive integer.")]
            public int Quantity { get; set; }
        }

        [KernelFunction]
        [Description("""  
            - Create a new order based on the user's request.
            - user can send 1 or more product to same order at [items] attribute
            - Return the [Data] ATTRIBUTE that comes from return value if successful, explain it as order Id.
            - In case you couldn't get any of required order attribute inform user to input or declare it except ProductId try to get it from last chat result.
            - In case [IsSuccess] ATTRIBUTE is false return the [Message] ATTRIBUTE that comes from return value to inform user about the failure reason.
            """)]
        public async Task<BaseResponse<int>> CreateOrder(
            [Required]
            [Description("User address where order will sent.")]
        string address,
            [Required]
            [Description("Products to be ordered. Quantity of each product default value is 1.,items must not be empty or null.")]
            List<ProductItemDto> items)
        {
            var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(new CreateOrderCommand()
            {
                Address = address,
                Items = items.Select(x => new CreateOrderItemDtos() { ProductId = x.ProductId, Quantity = x.Quantity }).ToList()
            });
        }
    }

}