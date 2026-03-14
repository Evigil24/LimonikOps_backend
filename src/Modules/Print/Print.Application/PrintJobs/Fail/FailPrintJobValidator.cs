using FluentValidation;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Fail;

internal sealed class FailPrintJobValidator : AbstractValidator<FailPrintJobCommand>
{
    public FailPrintJobValidator()
    {
        RuleFor(x => x.JobId).Must(id => id.Value != Guid.Empty).WithMessage("JobId is required.");
        RuleFor(x => x.AgentId).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FailedAtUtc).NotEqual(default(DateTime));
        RuleFor(x => x.AttemptNumber).GreaterThanOrEqualTo(0);
    }
}
