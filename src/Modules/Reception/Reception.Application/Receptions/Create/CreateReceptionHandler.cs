using LimonikOne.Modules.Reception.Domain.Receptions;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Reception.Application.Receptions.Create;

internal sealed class CreateReceptionHandler : ICommandHandler<CreateReceptionCommand, Guid>
{
    private readonly IReceptionRepository _receptionRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public CreateReceptionHandler(
        IReceptionRepository receptionRepository,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _receptionRepository = receptionRepository;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Result<Guid>> HandleAsync(CreateReceptionCommand command, CancellationToken cancellationToken = default)
    {
        var guestName = GuestName.Create(command.FirstName, command.LastName);
        var reception = ReceptionEntity.Create(guestName, command.Notes);

        await _receptionRepository.AddAsync(reception, cancellationToken);
        await _domainEventDispatcher.DispatchAsync(reception.DomainEvents, cancellationToken);
        reception.ClearDomainEvents();

        return Result.Success(reception.Id.Value);
    }
}
