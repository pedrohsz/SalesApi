using FluentValidation;
using SalesApi.Application.Dtos;

namespace SalesApi.Application.Validators
{
    public class CartDtoValidator : AbstractValidator<CartDto>
    {
        public CartDtoValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotEqual(Guid.Empty).WithMessage("UserId cannot be empty.");

            RuleFor(c => c.Items)
                .NotEmpty().WithMessage("Cart must contain at least one product.");
        }
    }
}
