using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Enqueue;

internal sealed class EnqueuePrintJobHandler(IPrintJobRepository repository)
    : ICommandHandler<EnqueuePrintJobCommand, Guid>
{
    private readonly IPrintJobRepository _repository = repository;

    public async Task<Result<Guid>> HandleAsync(
        EnqueuePrintJobCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var job = PrintJobEntity.Create(
            command.LogicalPrinterName,
            command.ZplPayload,
            command.Encoding,
            command.DocumentName,
            command.Priority,
            command.Metadata
        );

        await _repository.AddAsync(job, cancellationToken);

        return Result.Success(job.Id.Value);
    }
}
