using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Complete;

internal sealed class CompletePrintJobHandler : ICommandHandler<CompletePrintJobCommand>
{
    private readonly IPrintJobRepository _repository;

    public CompletePrintJobHandler(IPrintJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> HandleAsync(
        CompletePrintJobCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var job = await _repository.GetByIdAsync(command.JobId, cancellationToken);

        if (job is null)
            return Result.Failure(Error.NotFound("PrintJob.NotFound", "Print job not found."));

        var result = job.Complete(
            command.AgentId,
            command.CompletedAtUtc,
            command.WindowsPrinterName
        );

        if (result.IsFailure)
            return result;

        await _repository.UpdateAsync(job, cancellationToken);

        return Result.Success();
    }
}
