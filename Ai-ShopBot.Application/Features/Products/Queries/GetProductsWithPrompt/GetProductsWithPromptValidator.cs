using FluentValidation;

namespace Ai_ShopBot.Application.Features.Products.Queries.GetProductsWithPrompt
{
    public class GetProductsWithPromptValidator : AbstractValidator<GetProductsWithPromptQuery>
    {
        public GetProductsWithPromptValidator()
        {
            RuleFor(x => x.Limit)
                .LessThanOrEqualTo(20);

            RuleFor(x => x.Prompt)
                .MaximumLength(1000);
        }
    }
}
