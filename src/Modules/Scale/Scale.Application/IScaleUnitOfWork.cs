namespace LimonikOne.Modules.Scale.Application;

public interface IScaleUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
