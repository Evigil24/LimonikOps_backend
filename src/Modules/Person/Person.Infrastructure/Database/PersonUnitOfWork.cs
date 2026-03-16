using LimonikOne.Modules.Person.Application;

namespace LimonikOne.Modules.Person.Infrastructure.Database;

internal sealed class PersonUnitOfWork(PersonDbContext dbContext) : IPersonUnitOfWork
{
    private readonly PersonDbContext _dbContext = dbContext;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
