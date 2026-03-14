using FluentValidation;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Complete;

internal sealed class CompletePrintJobValidator : AbstractValidator<CompletePrintJobCommand>
{
    public CompletePrintJobValidator()
    {
        RuleFor(x => x.JobId).Must(id => id.Value != Guid.Empty).WithMessage("JobId is required.");
        RuleFor(x => x.AgentId).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CompletedAtUtc).NotEqual(default(DateTime));
    }
}
