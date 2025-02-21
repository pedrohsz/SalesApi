using FluentValidation;
using SalesApi.Application.Dtos;

namespace SalesApi.Application.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("Category is required.");

            RuleFor(p => p.Image)
                .NotEmpty().WithMessage("Image is required.");
        }
    }
}
