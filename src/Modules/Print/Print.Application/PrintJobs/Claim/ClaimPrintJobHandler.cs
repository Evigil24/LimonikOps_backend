using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Claim;

internal sealed class ClaimPrintJobHandler
    : ICommandHandler<ClaimPrintJobCommand, ClaimPrintJobResult?>
{
    private readonly IPrintJobRepository _repository;

    public ClaimPrintJobHandler(IPrintJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ClaimPrintJobResult?>> HandleAsync(
        ClaimPrintJobCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var job = await _repository.ClaimNextAsync(command.AgentId, cancellationToken);

        if (job is null)
            return Result.Success<ClaimPrintJobResult?>(null);

        var result = new ClaimPrintJobResult(
            job.Id.Value,
            job.LogicalPrinterName,
            job.ZplPayload,
            job.Encoding,
            job.DocumentName,
            job.QueuedAtUtc,
            job.Priority,
            job.Metadata
        );

        return Result.Success<ClaimPrintJobResult?>(result);
    }
}
