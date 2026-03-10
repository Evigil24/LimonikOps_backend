using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Fail;

public sealed record FailPrintJobCommand(
    PrintJobId JobId,
    string AgentId,
    DateTime FailedAtUtc,
    string? ErrorCode,
    string? ErrorMessage,
    string? StackTrace,
    bool Retryable,
    int AttemptNumber
) : ICommand;
