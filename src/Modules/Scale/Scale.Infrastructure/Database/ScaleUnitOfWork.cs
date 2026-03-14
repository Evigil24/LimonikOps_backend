using LimonikOne.Modules.Scale.Application;

namespace LimonikOne.Modules.Scale.Infrastructure.Database;

internal sealed class ScaleUnitOfWork : IScaleUnitOfWork
{
    private readonly ScaleDbContext _dbContext;

    public ScaleUnitOfWork(ScaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
