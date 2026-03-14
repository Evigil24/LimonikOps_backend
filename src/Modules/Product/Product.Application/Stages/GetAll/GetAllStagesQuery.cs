using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Stages.GetAll;

public sealed record GetAllStagesQuery : IQuery<IReadOnlyList<StageDto>>;
