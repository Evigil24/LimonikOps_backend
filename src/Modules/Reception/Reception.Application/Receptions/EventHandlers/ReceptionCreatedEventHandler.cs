using LimonikOne.Modules.Reception.Domain.Receptions.Events;
using LimonikOne.Shared.Abstractions.Domain;
using Microsoft.Extensions.Logging;

namespace LimonikOne.Modules.Reception.Application.Receptions.EventHandlers;

internal sealed class ReceptionCreatedEventHandler : IDomainEventHandler<ReceptionCreatedEvent>
{
    private readonly ILogger<ReceptionCreatedEventHandler> _logger;

    public ReceptionCreatedEventHandler(ILogger<ReceptionCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(ReceptionCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Reception created. ReceptionId: {ReceptionId}, Guest: {GuestFullName}",
            domainEvent.ReceptionId,
            domainEvent.GuestFullName);

        return Task.CompletedTask;
    }
}
