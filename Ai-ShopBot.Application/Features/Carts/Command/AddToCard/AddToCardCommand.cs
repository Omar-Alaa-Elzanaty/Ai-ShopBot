using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Core.DTOs;
using Ai_ShopBot.Core.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Net;
using System.Security.Claims;

namespace Ai_ShopBot.Application.Features.Carts.Command.AddToCard
{
    public sealed class AddToCardCommand : IRequest<BaseResponse<bool>>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    internal class AddToCardCommandHandler : IRequestHandler<AddToCardCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;

        public AddToCardCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<BaseResponse<bool>> Handle(AddToCardCommand request, CancellationToken cancellationToken)
        {
            var userid = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var cart = request.Adapt<Cart>();
            cart.ClientId = userid;

            await _unitOfWork.CartsRepo.AddAsync(cart);
            await _unitOfWork.SaveAsync(cancellationToken);

            return BaseResponse<bool>.Success("Product add to cart.");
        }
    }
}
