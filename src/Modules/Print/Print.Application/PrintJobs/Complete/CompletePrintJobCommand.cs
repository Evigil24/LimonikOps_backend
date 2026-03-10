using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Complete;

public sealed record CompletePrintJobCommand(
    PrintJobId JobId,
    string AgentId,
    DateTime CompletedAtUtc,
    string? WindowsPrinterName
) : ICommand;
