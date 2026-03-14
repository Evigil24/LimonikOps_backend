using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Print.Domain.PrintJobs;

public sealed class PrintJobEntity : AggregateRoot<PrintJobId>
{
    public string LogicalPrinterName { get; private set; } = null!;
    public string ZplPayload { get; private set; } = null!;
    public string? Encoding { get; private set; }
    public string? DocumentName { get; private set; }
    public DateTime QueuedAtUtc { get; private set; }
    public int Priority { get; private set; }
    public Dictionary<string, string>? Metadata { get; private set; }
    public PrintJobStatus Status { get; private set; }
    public string? ClaimedByAgentId { get; private set; }
    public DateTime? ClaimedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? WindowsPrinterName { get; private set; }
    public DateTime? FailedAtUtc { get; private set; }
    public string? ErrorCode { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? StackTrace { get; private set; }
    public bool Retryable { get; private set; }
    public int AttemptNumber { get; private set; }

    private PrintJobEntity() { }

    public static PrintJobEntity Create(
        string logicalPrinterName,
        string zplPayload,
        string? encoding,
        string? documentName,
        int priority,
        Dictionary<string, string>? metadata
    )
    {
        return new PrintJobEntity
        {
            Id = PrintJobId.New(),
            LogicalPrinterName = logicalPrinterName,
            ZplPayload = zplPayload,
            Encoding = encoding,
            DocumentName = documentName,
            QueuedAtUtc = DateTime.UtcNow,
            Priority = priority,
            Metadata = metadata,
            Status = PrintJobStatus.Queued,
            AttemptNumber = 0,
            Retryable = false,
        };
    }

    public Result Claim(string agentId)
    {
        if (Status != PrintJobStatus.Queued)
            return Result.Failure(
                Error.Conflict("PrintJob.NotQueued", "Only queued jobs can be claimed.")
            );

        Status = PrintJobStatus.Claimed;
        ClaimedByAgentId = agentId;
        ClaimedAtUtc = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Complete(string agentId, DateTime completedAtUtc, string? windowsPrinterName)
    {
        if (Status == PrintJobStatus.Completed)
            return Result.Success();

        if (Status != PrintJobStatus.Claimed)
            return Result.Failure(
                Error.Conflict("PrintJob.NotClaimed", "Only claimed jobs can be completed.")
            );

        Status = PrintJobStatus.Completed;
        CompletedAtUtc = completedAtUtc;
        WindowsPrinterName = windowsPrinterName;
        return Result.Success();
    }

    public Result Fail(
        string agentId,
        DateTime failedAtUtc,
        string? errorCode,
        string? errorMessage,
        string? stackTrace,
        bool retryable,
        int attemptNumber
    )
    {
        if (Status == PrintJobStatus.Failed)
            return Result.Success();

        if (Status != PrintJobStatus.Claimed)
            return Result.Failure(
                Error.Conflict("PrintJob.NotClaimed", "Only claimed jobs can be failed.")
            );

        Status = PrintJobStatus.Failed;
        FailedAtUtc = failedAtUtc;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        StackTrace = stackTrace;
        Retryable = retryable;
        AttemptNumber = attemptNumber;
        return Result.Success();
    }

    public Result RequeueForRetry()
    {
        if (Status != PrintJobStatus.Failed || !Retryable)
            return Result.Failure(
                Error.Conflict(
                    "PrintJob.NotRetryable",
                    "Only failed retryable jobs can be requeued."
                )
            );

        Status = PrintJobStatus.Queued;
        AttemptNumber++;
        ClaimedByAgentId = null;
        ClaimedAtUtc = null;
        FailedAtUtc = null;
        ErrorCode = null;
        ErrorMessage = null;
        StackTrace = null;
        Retryable = false;
        return Result.Success();
    }
}
