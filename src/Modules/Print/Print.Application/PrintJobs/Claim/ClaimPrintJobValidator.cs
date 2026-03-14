using FluentValidation;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Claim;

internal sealed class ClaimPrintJobValidator : AbstractValidator<ClaimPrintJobCommand>
{
    public ClaimPrintJobValidator()
    {
        RuleFor(x => x.AgentId).NotEmpty().MaximumLength(100);
    }
}
