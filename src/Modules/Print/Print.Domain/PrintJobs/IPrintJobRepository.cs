namespace LimonikOne.Modules.Print.Domain.PrintJobs;

public interface IPrintJobRepository
{
    Task<PrintJobEntity?> ClaimNextAsync(string agentId, CancellationToken ct = default);
    Task<PrintJobEntity?> GetByIdAsync(PrintJobId id, CancellationToken ct = default);
    Task AddAsync(PrintJobEntity job, CancellationToken ct = default);
    Task UpdateAsync(PrintJobEntity job, CancellationToken ct = default);
}
