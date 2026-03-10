namespace LimonikOne.Modules.Print.Application.PrintJobs.Claim;

public sealed record ClaimPrintJobResult(
    Guid JobId,
    string LogicalPrinterName,
    string ZplPayload,
    string? Encoding,
    string? DocumentName,
    DateTime QueuedAtUtc,
    int Priority,
    Dictionary<string, string>? Metadata
);
