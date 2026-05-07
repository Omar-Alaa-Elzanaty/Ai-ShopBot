using Ai_ShopBot.Core.DTOs;
using FluentValidation;
using MediatR;

namespace Ai_ShopBot.Application.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next(cancellationToken);
            }

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

            var errors = validationResults
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationResult => validationResult is not null)
                .Distinct()
                .ToList();

            if (errors.Count != 0)
            {
                return (BaseResponse<bool>.ValidationFailure(errors) as TResponse)!;
            }

            return await next(cancellationToken);
        }

    }
}
