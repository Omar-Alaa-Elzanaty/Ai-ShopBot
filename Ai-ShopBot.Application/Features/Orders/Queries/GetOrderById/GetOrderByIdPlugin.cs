using Ai_ShopBot.Croe.DTOs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById
{
    public sealed class GetOrderByIdPlugin
    {
        private readonly IServiceProvider _serviceProvider;

        public GetOrderByIdPlugin(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [KernelFunction]
        [Description($"""
            Get order details by order Id
                - In case [IsSuccess] Attribute true Display [Data] Attribute in card shap
                - In case [IsSuccess] Attribute false Diaply [Message] Attribute, to declare problem
            """)]
        public async Task<BaseResponse<List<GetOrderByIdQueryDto>>> GetOrderDetails(int orderId)
        {
            var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(new GetOrderByIdQuery(orderId));
        }
    }
}
