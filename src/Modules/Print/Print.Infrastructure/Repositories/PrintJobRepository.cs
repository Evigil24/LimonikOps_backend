using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Modules.Print.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Print.Infrastructure.Repositories;

internal sealed class PrintJobRepository : IPrintJobRepository
{
    private readonly PrintDbContext _dbContext;

    public PrintJobRepository(PrintDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PrintJobEntity?> ClaimNextAsync(
        string agentId,
        CancellationToken ct = default
    )
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

        var job = await _dbContext
            .PrintJobs.FromSqlRaw(
                """
                SELECT * FROM print.print_jobs
                WHERE status = 'Queued'
                ORDER BY priority ASC, queued_at_utc ASC
                LIMIT 1
                FOR UPDATE SKIP LOCKED
                """
            )
            .FirstOrDefaultAsync(ct);

        if (job is null)
        {
            await transaction.CommitAsync(ct);
            return null;
        }

        job.Claim(agentId);
        await _dbContext.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return job;
    }

    public async Task<PrintJobEntity?> GetByIdAsync(PrintJobId id, CancellationToken ct = default)
    {
        return await _dbContext.PrintJobs.FirstOrDefaultAsync(j => j.Id == id, ct);
    }

    public async Task AddAsync(PrintJobEntity job, CancellationToken ct = default)
    {
        await _dbContext.PrintJobs.AddAsync(job, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PrintJobEntity job, CancellationToken ct = default)
    {
        _dbContext.PrintJobs.Update(job);
        await _dbContext.SaveChangesAsync(ct);
    }
}
