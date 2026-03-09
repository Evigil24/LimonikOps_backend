using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Reception.Application.Receptions.Get;

public sealed record GetReceptionByIdQuery(Guid Id) : IQuery<ReceptionDto>;
