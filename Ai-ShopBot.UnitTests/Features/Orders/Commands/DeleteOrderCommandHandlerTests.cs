using Ai_ShopBot.Application.Features.Orders.Commands.Delete;
using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Core.DTOs;
using Ai_ShopBot.Core.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Security.Claims;

namespace Ai_ShopBot.UnitTests.Features.Orders.Commands
{
    public class DeleteOrderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly DeleteOrderCommandHandler _handler;

        public DeleteOrderCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpContextMock = new Mock<IHttpContextAccessor>();
            _orderRepoMock = new Mock<IOrderRepository>();

            _unitOfWorkMock.Setup(x => x.OrdersRepo).Returns(_orderRepoMock.Object);

            _handler = new DeleteOrderCommandHandler(_unitOfWorkMock.Object, _httpContextMock.Object);
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
        public async Task Handle_ShouldDeleteOrder_WhenOrderExistsAndOwnedByUser()
        {
            var userId = "user-1";
            var orderId = 1;
            var order = new Order { Id = orderId, ClientId = userId };

            SetupHttpContext(userId);
            _orderRepoMock.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);

            var result = await _handler.Handle(new DeleteOrderCommand { Id = orderId }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().BeTrue();
            result.Message.Should().Be("Order deleted successfully");
            _orderRepoMock.Verify(x => x.Delete(order), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            SetupHttpContext("user-1");
            _orderRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order?)null);

            var result = await _handler.Handle(new DeleteOrderCommand { Id = 999 }, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("Order not found");
            _orderRepoMock.Verify(x => x.Delete(It.IsAny<Order>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnForbidden_WhenOrderBelongsToDifferentUser()
        {
            var orderId = 1;
            var order = new Order { Id = orderId, ClientId = "other-user" };

            SetupHttpContext("current-user");
            _orderRepoMock.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);

            var result = await _handler.Handle(new DeleteOrderCommand { Id = orderId }, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            result.Message.Should().Be("You are not authorized to delete this order");
            _orderRepoMock.Verify(x => x.Delete(It.IsAny<Order>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
