using FluentValidation;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Enqueue;

internal sealed class EnqueuePrintJobValidator : AbstractValidator<EnqueuePrintJobCommand>
{
    public EnqueuePrintJobValidator()
    {
        RuleFor(x => x.LogicalPrinterName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ZplPayload).NotEmpty();
        RuleFor(x => x.Priority).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Encoding).MaximumLength(50).When(x => x.Encoding is not null);
        RuleFor(x => x.DocumentName).MaximumLength(500).When(x => x.DocumentName is not null);
    }
}
