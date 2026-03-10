using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Print.Application.PrintJobs.Claim;

public sealed record ClaimPrintJobCommand(string AgentId) : ICommand<ClaimPrintJobResult?>;
