using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Reception.Domain.Receptions;

public sealed class GuestName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    private GuestName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static GuestName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        return new GuestName(firstName.Trim(), lastName.Trim());
    }

    public string FullName => $"{FirstName} {LastName}";

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
    }
}
