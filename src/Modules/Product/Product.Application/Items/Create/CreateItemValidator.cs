using FluentValidation;
using LimonikOne.Modules.Product.Domain.Items;

namespace LimonikOne.Modules.Product.Application.Items.Create;

internal sealed class CreateItemValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.ItemNumber)
            .NotEmpty()
            .WithMessage("Item number is required.")
            .MaximumLength(100)
            .WithMessage("Item number must not exceed 100 characters.");

        RuleFor(x => x.PrimaryName)
            .NotEmpty()
            .WithMessage("Primary name is required.")
            .MaximumLength(300)
            .WithMessage("Primary name must not exceed 300 characters.");

        RuleFor(x => x.SearchName)
            .NotEmpty()
            .WithMessage("Search name is required.")
            .MaximumLength(500)
            .WithMessage("Search name must not exceed 500 characters.");

        RuleFor(x => x.VarietyId)
            .Must(id => Variety.All.Any(v => v.Id == id))
            .WithMessage("Invalid variety ID.");

        RuleFor(x => x.HandlingId)
            .Must(id => Handling.All.Any(h => h.Id == id))
            .WithMessage("Invalid handling ID.");

        RuleFor(x => x.CertificationId)
            .Must(id => Certification.All.Any(c => c.Id == id))
            .WithMessage("Invalid certification ID.");

        RuleFor(x => x.StageId)
            .Must(id => Stage.All.Any(s => s.Id == id))
            .WithMessage("Invalid stage ID.");
    }
}
