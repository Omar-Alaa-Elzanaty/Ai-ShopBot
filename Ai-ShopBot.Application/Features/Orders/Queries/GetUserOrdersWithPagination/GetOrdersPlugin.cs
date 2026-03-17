using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Ai_ShopBot.Application.Features.Orders.Queries.GetUserOrdersWithPagination
{
    public sealed class GetOrdersPlugin
    {
        private readonly IServiceProvider _serviceProvider;

        public GetOrdersPlugin(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [KernelFunction]
        [Description("""
            Get Orders made by user
                - Display return List like a card.
                - In case no Data return Inform user that he/she doesn't have any order yet.
                - Display [Id] Attribute of order so user can benfit from it again.
            """)]
        public async Task<IEnumerable<GetUserOrdersWithPaginationQueryDto>> GetOrders(
            [Description("Number of page that data will come from,In case say next or so on depend on chat history and increase PageNumber value by 1,Default value 1")]
            int pageNumber,
            [Description("Number of return pages,Default value 5")]
            int pageSize)
        {
            var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var response = await mediator.Send(new GetUserOrdersWithPaginationQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return response.Data;
        }
    }
}
