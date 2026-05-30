using Ai_ShopBot.Core.DTOs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Ai_ShopBot.Application.Features.Orders.Commands.Delete
{
    public class DeleteOrderPlugin
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteOrderPlugin(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [KernelFunction]
        [Description("""
            Delete an order by its Id.
            - Only the owner of the order can delete it.
            - In case [IsSuccess] ATTRIBUTE is false return the [Message] ATTRIBUTE that comes from return value to inform user about the failure reason.
            """)]
        public async Task<BaseResponse<bool>> DeleteOrder(
            [Description("The Id of the order to delete.")]
        int orderId)
        {
            var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(new DeleteOrderCommand()
            {
                Id = orderId
            });
        }
    }
}
