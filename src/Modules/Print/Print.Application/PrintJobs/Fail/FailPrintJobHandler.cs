using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Fail;

internal sealed class FailPrintJobHandler : ICommandHandler<FailPrintJobCommand>
{
    private readonly IPrintJobRepository _repository;

    public FailPrintJobHandler(IPrintJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> HandleAsync(
        FailPrintJobCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var job = await _repository.GetByIdAsync(command.JobId, cancellationToken);

        if (job is null)
            return Result.Failure(Error.NotFound("PrintJob.NotFound", "Print job not found."));

        var result = job.Fail(
            command.AgentId,
            command.FailedAtUtc,
            command.ErrorCode,
            command.ErrorMessage,
            command.StackTrace,
            command.Retryable,
            command.AttemptNumber
        );

        if (result.IsFailure)
            return result;

        if (command.Retryable)
        {
            var requeueResult = job.RequeueForRetry();
            if (requeueResult.IsFailure)
                return requeueResult;
        }

        await _repository.UpdateAsync(job, cancellationToken);

        return Result.Success();
    }
}
