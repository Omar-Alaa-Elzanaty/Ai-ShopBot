using Ai_ShopBot.Application.Features.Orders.Commands.Create;
using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Core.Models;
using FluentAssertions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Security.Claims;

namespace Ai_ShopBot.UnitTests.Features.Orders.Commands
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpContextMock = new Mock<IHttpContextAccessor>();
            _orderRepoMock = new Mock<IOrderRepository>();

            _unitOfWorkMock.Setup(x => x.OrdersRepo).Returns(_orderRepoMock.Object);

            _handler = new CreateOrderCommandHandler(_unitOfWorkMock.Object, _httpContextMock.Object);
        }

        private void SetupHttpContext(string userId)
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContext);
        }

        [Fact]
        public async Task Handle_ShouldCreateOrder_WhenValidRequest()
        {
            var userId = "user-1";
            SetupHttpContext(userId);

            var command = new CreateOrderCommand
            {
                Address = "123 Main St",
                Items = new List<CreateOrderItemDtos>
                {
                    new() { ProductId = "prod-1", Quantity = 2 }
                }
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Message.Should().Be("Order created successfully");
            _orderRepoMock.Verify(x => x.AddAsync(It.Is<Order>(o =>
                o.ClientId == userId &&
                o.Address == command.Address)), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
