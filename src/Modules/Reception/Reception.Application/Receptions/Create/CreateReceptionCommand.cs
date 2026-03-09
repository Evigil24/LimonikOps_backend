using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Reception.Application.Receptions.Create;

public sealed record CreateReceptionCommand(
    string FirstName,
    string LastName,
    string? Notes) : ICommand<Guid>;
