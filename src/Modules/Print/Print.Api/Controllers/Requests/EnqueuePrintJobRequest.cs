namespace LimonikOne.Modules.Print.Api.Controllers.Requests;

public sealed record EnqueuePrintJobRequest(
    string LogicalPrinterName,
    string ZplPayload,
    string? Encoding,
    string? DocumentName,
    int Priority,
    Dictionary<string, string>? Metadata
);
