using FluentValidation;

namespace LimonikOne.Modules.Scale.Application.Weights.Ingest;

public sealed class IngestWeightBatchValidator : AbstractValidator<IngestWeightBatchCommand>
{
    public IngestWeightBatchValidator()
    {
        RuleFor(x => x.BatchId).NotEmpty().WithMessage("Batch ID is required.");

        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .WithMessage("Device ID is required.")
            .MaximumLength(100)
            .WithMessage("Device ID must not exceed 100 characters.");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required.")
            .MaximumLength(200)
            .WithMessage("Location must not exceed 200 characters.");

        RuleFor(x => x.SentAt).NotEqual(default(DateTime)).WithMessage("Sent at is required.");

        RuleFor(x => x.Readings)
            .NotEmpty()
            .WithMessage("At least one reading is required.")
            .Must(r => r.Count <= 10000)
            .WithMessage("Readings must not exceed 10,000 items.");

        RuleForEach(x => x.Readings)
            .ChildRules(reading =>
            {
                reading
                    .RuleFor(r => r.Weight)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Weight must be zero or positive.");

                reading
                    .RuleFor(r => r.Count)
                    .GreaterThan(0)
                    .WithMessage("Count must be greater than zero.");

                reading
                    .RuleFor(r => r.StableCount)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Stable count must be zero or positive.");

                reading
                    .RuleFor(r => r)
                    .Must(r => r.FirstTimestamp <= r.LastTimestamp)
                    .WithMessage("First timestamp must be before or equal to last timestamp.");
            });
    }
}
