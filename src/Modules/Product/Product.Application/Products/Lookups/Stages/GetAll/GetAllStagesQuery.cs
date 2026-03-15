using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.Lookups.Stages.GetAll;

public sealed record GetAllStagesQuery : IQuery<IReadOnlyList<StageDto>>;
