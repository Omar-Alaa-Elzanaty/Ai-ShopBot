using Ai_ShopBot.Croe.DTOs;
using Ai_ShopBot.Croe.Interfaces;
using Ai_ShopBot.Croe.Models;
using Mapster;
using MediatR;
using Microsoft.Extensions.AI;
using MongoDB.Driver;


namespace Ai_ShopBot.Application.Features.Products.Queries.GetProductsWithPrompt
{
    public sealed record GetProductsWithPromptQuery : IRequest<BaseResponse<List<GetProductsWithPromptQueryDto>>>
    {
        public string? Prompt { get; set; }
        public int Limit { get; set; } = 10;
    }

    internal class GetProductWithPromptQueryHandler : IRequestHandler<GetProductsWithPromptQuery, BaseResponse<List<GetProductsWithPromptQueryDto>>>
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public GetProductWithPromptQueryHandler(
            IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
            IUnitOfWork unitOfWork)
        {
            _embeddingGenerator = embeddingGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<List<GetProductsWithPromptQueryDto>>> Handle(
            GetProductsWithPromptQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                var products = await _unitOfWork.ProductsRepo.Entities
                    .Find(Builders<Product>.Filter.Empty)
                    .Limit(request.Limit)
                    .ToListAsync(cancellationToken);

                return BaseResponse<List<GetProductsWithPromptQueryDto>>.Success(products.Adapt<List<GetProductsWithPromptQueryDto>>() ?? []);
            }


            var embeddingPrompt = await _embeddingGenerator.GenerateAsync(request.Prompt,
                new EmbeddingGenerationOptions
                {
                    Dimensions = 1024,
                }, cancellationToken: cancellationToken);

            var vectorSearchOptions = new VectorSearchOptions<Product>
            {
                IndexName = "vector_index",
                NumberOfCandidates = request.Limit * 20,
            };


            var results = await _unitOfWork.ProductsRepo.Entities
                .Aggregate()
                .VectorSearch(p => p.PlotEmbedding1024, embeddingPrompt.Vector.ToArray(), request.Limit, vectorSearchOptions)
                .ToListAsync(cancellationToken);


            return BaseResponse<List<GetProductsWithPromptQueryDto>>.Success(results.Adapt<List<GetProductsWithPromptQueryDto>>());
        }
    }
}
