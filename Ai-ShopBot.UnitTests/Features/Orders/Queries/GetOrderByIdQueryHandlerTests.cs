using Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById;
using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Core.DTOs;
using Ai_ShopBot.Core.DTOs.ProductRepo;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Security.Claims;

namespace Ai_ShopBot.UnitTests.Features.Orders.Queries
{
    public class GetOrderByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOrderByIdQueryHandler _handler;

        public GetOrderByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpContextMock = new Mock<IHttpContextAccessor>();
            _orderRepoMock = new Mock<IOrderRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(x => x.OrdersRepo).Returns(_orderRepoMock.Object);
            _unitOfWorkMock.Setup(x => x.ProductsRepo).Returns(_productRepoMock.Object);

            _handler = new GetOrderByIdQueryHandler(
                _unitOfWorkMock.Object, _httpContextMock.Object, _mapperMock.Object);
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
        public async Task Handle_ShouldReturnOrderItems_WhenOrderExists()
        {
            var userId = "user-1";
            var orderId = 1;
            SetupHttpContext(userId);

            var orderItems = new List<GetOrderByIdQueryDto>
            {
                new() { ProductId = "prod-1", Quantity = 2, Name = "Product 1", Color = "Red" }
            };

            var products = new List<ProductForOrderItemDto>
            {
                new() { Id = "prod-1", Name = "Product 1", Color = "Red" }
            };

            _orderRepoMock.Setup(x => x.GetUserOrderItemsByOrderId(orderId, userId))
                .ReturnsAsync(orderItems);
            _productRepoMock.Setup(x => x.GetProductsForOrderItem(It.IsAny<List<string>>()))
                .ReturnsAsync(products);

            var result = await _handler.Handle(new GetOrderByIdQuery(orderId), CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().HaveCount(1);
            result.Data![0].ProductId.Should().Be("prod-1");
            _mapperMock.Verify(x => x.Map(products[0], orderItems[0]), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenOrderNotFound()
        {
            SetupHttpContext("user-1");
            _orderRepoMock.Setup(x => x.GetUserOrderItemsByOrderId(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new List<GetOrderByIdQueryDto>());

            var result = await _handler.Handle(new GetOrderByIdQuery(999), CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Order not found or you may have no access to this order.");
        }
    }
}
