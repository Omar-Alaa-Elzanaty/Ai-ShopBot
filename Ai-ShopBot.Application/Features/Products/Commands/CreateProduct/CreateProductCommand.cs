using Ai_ShopBot.Croe.DTOs;
using Ai_ShopBot.Croe.Enums;
using Ai_ShopBot.Croe.Interfaces;
using Ai_ShopBot.Croe.Models;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.AI;

namespace Ai_ShopBot.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<BaseResponse<string>>
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public ProductSizes Size { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, BaseResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateProductCommand> _validator;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

        public CreateProductCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<CreateProductCommand> validator,
            IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _embeddingGenerator = embeddingGenerator;
        }

        public async Task<BaseResponse<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BaseResponse<string>.ValidationFailure(validationResult.Errors.ToList());
            }

            var product = request.Adapt<Product>();
            product!.PlotEmbedding1024 = await GeneratePlotEmbedding(request.Description);

            await _unitOfWork.ProductsRepo.AddAsync(product);

            return BaseResponse<string>.Success(message: "Product created successfully");
        }

        async Task<float[]> GeneratePlotEmbedding(string description)
        {
            var generatedEmbeddings = await _embeddingGenerator.GenerateAsync(description,
                new EmbeddingGenerationOptions
                {
                    Dimensions = 1024
                });

            return generatedEmbeddings.Vector.ToArray();
        }
    }
}
