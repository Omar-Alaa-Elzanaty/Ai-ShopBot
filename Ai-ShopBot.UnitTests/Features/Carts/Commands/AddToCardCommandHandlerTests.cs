using Ai_ShopBot.Application.Features.Carts.Command.AddToCard;
using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.Models;
using FluentAssertions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Security.Claims;

namespace Ai_ShopBot.UnitTests.Features.Carts.Commands
{
    public class AddToCardCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly Mock<IBaseRepository<Cart>> _cartRepoMock;
        private readonly AddToCardCommandHandler _handler;

        public AddToCardCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpContextMock = new Mock<IHttpContextAccessor>();
            _cartRepoMock = new Mock<IBaseRepository<Cart>>();

            _unitOfWorkMock.Setup(x => x.CartsRepo).Returns(_cartRepoMock.Object);

            _handler = new AddToCardCommandHandler(_unitOfWorkMock.Object, _httpContextMock.Object);
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
        public async Task Handle_ShouldAddProductToCart_WhenValidRequest()
        {
            var userId = "user-1";
            SetupHttpContext(userId);

            var command = new AddToCardCommand
            {
                ProductId = "507f1f77bcf86cd799439011",
                Quantity = 1
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Message.Should().Be("Product add to cart.");
            _cartRepoMock.Verify(x => x.AddAsync(It.Is<Cart>(c =>
                c.ClientId == userId &&
                c.ProductId == command.ProductId &&
                c.Quantity == command.Quantity)), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
