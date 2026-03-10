namespace LimonikOne.Modules.Print.Api.Controllers.Requests;

public sealed record CompletePrintJobRequest(
    string AgentId,
    DateTime CompletedAtUtc,
    string? WindowsPrinterName
);
