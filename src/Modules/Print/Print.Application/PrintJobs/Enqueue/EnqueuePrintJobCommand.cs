using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Enqueue;

public sealed record EnqueuePrintJobCommand(
    string LogicalPrinterName,
    string ZplPayload,
    string? Encoding,
    string? DocumentName,
    int Priority,
    Dictionary<string, string>? Metadata
) : ICommand<Guid>;
