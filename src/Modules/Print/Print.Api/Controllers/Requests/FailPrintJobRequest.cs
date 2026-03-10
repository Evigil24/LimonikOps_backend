namespace LimonikOne.Modules.Print.Api.Controllers.Requests;

public sealed record FailPrintJobRequest(
    string AgentId,
    DateTime FailedAtUtc,
    string? ErrorCode,
    string? ErrorMessage,
    string? StackTrace,
    bool Retryable,
    int AttemptNumber
);
