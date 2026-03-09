using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Reception.Domain.Receptions.Events;

public sealed record ReceptionCreatedEvent(ReceptionId ReceptionId, string GuestFullName) : DomainEvent;
