using FluentValidation;
using SalesApi.Application.Dtos;

namespace SalesApi.Application.Validators
{
    public class SaleDtoValidator : AbstractValidator<SaleDto>
    {
        public SaleDtoValidator()
        {
            RuleFor(s => s.SaleNumber)
                .NotEmpty().WithMessage("SaleNumber is required.")
                .MaximumLength(50).WithMessage("SaleNumber cannot exceed 50 characters.");

            RuleFor(s => s.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.")
                .NotEqual(Guid.Empty).WithMessage("CustomerId cannot be empty.");

            RuleFor(s => s.BranchId)
                .NotEmpty().WithMessage("BranchId is required.")
                .NotEqual(Guid.Empty).WithMessage("BranchId cannot be empty.");

            RuleFor(s => s.Items)
                .NotEmpty().WithMessage("Sale must contain at least one item.");
        }
    }
}
