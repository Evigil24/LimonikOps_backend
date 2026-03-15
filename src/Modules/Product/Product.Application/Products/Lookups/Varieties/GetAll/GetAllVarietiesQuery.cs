using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.Lookups.Varieties.GetAll;

public sealed record GetAllVarietiesQuery : IQuery<IReadOnlyList<VarietyDto>>;
