using LimonikOne.Modules.Person.Application;

namespace LimonikOne.Modules.Person.Infrastructure.Database;

internal sealed class PersonUnitOfWork : IPersonUnitOfWork
{
    private readonly PersonDbContext _dbContext;

    public PersonUnitOfWork(PersonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
