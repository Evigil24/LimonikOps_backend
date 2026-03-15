namespace LimonikOne.Modules.Person.Application;

public interface IPersonUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
