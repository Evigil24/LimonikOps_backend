using LimonikOne.Modules.Reception.Domain.Receptions;
using LimonikOne.Modules.Reception.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Reception.Infrastructure.Repositories;

internal sealed class ReceptionRepository : IReceptionRepository
{
    private readonly ReceptionDbContext _dbContext;

    public ReceptionRepository(ReceptionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReceptionEntity?> GetByIdAsync(ReceptionId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Receptions
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task AddAsync(ReceptionEntity reception, CancellationToken cancellationToken = default)
    {
        await _dbContext.Receptions.AddAsync(reception, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
