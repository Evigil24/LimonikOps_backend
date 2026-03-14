using LimonikOne.Modules.Product.Domain.Products;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Stages.GetAll;

internal sealed class GetAllStagesHandler
    : IQueryHandler<GetAllStagesQuery, IReadOnlyList<StageDto>>
{
    public Task<Result<IReadOnlyList<StageDto>>> HandleAsync(
        GetAllStagesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var stages = Stage
            .All.Select(s => new StageDto(s.Id, s.Name, s.Label, s.ShortName, s.Description))
            .ToList();

        return Task.FromResult(Result.Success<IReadOnlyList<StageDto>>(stages));
    }
}
