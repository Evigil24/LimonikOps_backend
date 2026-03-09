namespace LimonikOne.Modules.Reception.Domain.Receptions;

public interface IReceptionRepository
{
    Task<ReceptionEntity?> GetByIdAsync(ReceptionId id, CancellationToken cancellationToken = default);
    Task AddAsync(ReceptionEntity reception, CancellationToken cancellationToken = default);
}
