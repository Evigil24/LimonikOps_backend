using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Handlings.GetAll;

public sealed record GetAllHandlingsQuery : IQuery<IReadOnlyList<HandlingDto>>;
