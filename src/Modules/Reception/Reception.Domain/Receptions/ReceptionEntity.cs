using LimonikOne.Modules.Reception.Domain.Receptions.Events;
using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Reception.Domain.Receptions;

public sealed class ReceptionEntity : AggregateRoot<ReceptionId>
{
    public GuestName GuestName { get; private set; } = null!;
    public ReceptionStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CheckedInAt { get; private set; }

    private ReceptionEntity() { } // EF Core

    public static ReceptionEntity Create(GuestName guestName, string? notes)
    {
        var reception = new ReceptionEntity
        {
            Id = ReceptionId.New(),
            GuestName = guestName,
            Status = ReceptionStatus.Pending,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };

        reception.RaiseDomainEvent(new ReceptionCreatedEvent(reception.Id, guestName.FullName));

        return reception;
    }

    public void CheckIn()
    {
        if (Status != ReceptionStatus.Pending)
            throw new InvalidOperationException($"Cannot check in a reception with status '{Status}'.");

        Status = ReceptionStatus.CheckedIn;
        CheckedInAt = DateTime.UtcNow;
    }
}
