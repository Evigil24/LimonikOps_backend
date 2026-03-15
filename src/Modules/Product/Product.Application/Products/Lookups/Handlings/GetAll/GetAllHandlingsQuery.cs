using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.Lookups.Handlings.GetAll;

public sealed record GetAllHandlingsQuery : IQuery<IReadOnlyList<HandlingDto>>;
