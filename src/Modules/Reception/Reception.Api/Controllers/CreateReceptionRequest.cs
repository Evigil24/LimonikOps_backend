namespace LimonikOne.Modules.Reception.Api.Controllers;

public sealed record CreateReceptionRequest(string FirstName, string LastName, string? Notes);
