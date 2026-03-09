using FluentAssertions;
using LimonikOne.Modules.Reception.Domain.Receptions;
using LimonikOne.Modules.Reception.Domain.Receptions.Events;

namespace LimonikOne.Modules.Reception.UnitTests.Domain;

public class ReceptionEntityTests
{
    [Fact]
    public void Create_Should_Set_Properties_And_Raise_DomainEvent()
    {
        // Arrange
        var guestName = GuestName.Create("John", "Doe");

        // Act
        var reception = ReceptionEntity.Create(guestName, "Some notes");

        // Assert
        reception.Id.Value.Should().NotBe(Guid.Empty);
        reception.GuestName.FirstName.Should().Be("John");
        reception.GuestName.LastName.Should().Be("Doe");
        reception.Status.Should().Be(ReceptionStatus.Pending);
        reception.Notes.Should().Be("Some notes");
        reception.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        reception.CheckedInAt.Should().BeNull();

        reception.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ReceptionCreatedEvent>();
    }

    [Fact]
    public void Create_Should_Raise_ReceptionCreatedEvent_With_CorrectData()
    {
        // Arrange
        var guestName = GuestName.Create("Jane", "Smith");

        // Act
        var reception = ReceptionEntity.Create(guestName, null);

        // Assert
        var domainEvent = reception.DomainEvents.Single() as ReceptionCreatedEvent;
        domainEvent.Should().NotBeNull();
        domainEvent!.ReceptionId.Should().Be(reception.Id);
        domainEvent.GuestFullName.Should().Be("Jane Smith");
    }

    [Fact]
    public void CheckIn_Should_Update_Status_And_CheckedInAt()
    {
        // Arrange
        var guestName = GuestName.Create("John", "Doe");
        var reception = ReceptionEntity.Create(guestName, null);

        // Act
        reception.CheckIn();

        // Assert
        reception.Status.Should().Be(ReceptionStatus.CheckedIn);
        reception.CheckedInAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ClearDomainEvents_Should_Remove_All_Events()
    {
        // Arrange
        var guestName = GuestName.Create("John", "Doe");
        var reception = ReceptionEntity.Create(guestName, null);

        // Act
        reception.ClearDomainEvents();

        // Assert
        reception.DomainEvents.Should().BeEmpty();
    }
}
