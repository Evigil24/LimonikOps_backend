namespace LimonikOne.Modules.Reception.Application.Receptions.Get;

public sealed record ReceptionDto(
    Guid Id,
    long DisplayId,
    string FirstName,
    string LastName,
    string Status,
    string? Notes,
    DateTime CreatedAt,
    DateTime? CheckedInAt);
