using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.Lookups.Stages.GetAll;

public sealed record GetAllStagesQuery : IQuery<IReadOnlyList<StageDto>>;
