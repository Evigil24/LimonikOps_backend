using FluentValidation;
using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Create;

internal sealed class CreateVehicleScaleRecordValidator
    : AbstractValidator<CreateVehicleScaleRecordCommand>
{
    public CreateVehicleScaleRecordValidator()
    {
        RuleFor(x => x.TypeId)
            .Must(id => VehicleScaleRecordType.All.Any(type => type.Id == id))
            .WithMessage("Invalid vehicle scale record type ID.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("Created by is required.")
            .MaximumLength(100)
            .WithMessage("Created by must not exceed 100 characters.");
    }
}
