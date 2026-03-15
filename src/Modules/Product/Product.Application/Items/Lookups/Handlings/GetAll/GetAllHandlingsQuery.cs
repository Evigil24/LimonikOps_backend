using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.Lookups.Handlings.GetAll;

public sealed record GetAllHandlingsQuery : IQuery<IReadOnlyList<HandlingDto>>;
