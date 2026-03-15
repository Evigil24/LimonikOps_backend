using LimonikOne.Modules.Product.Domain.Items;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.Lookups.Varieties.GetAll;

internal sealed class GetAllVarietiesHandler
    : IQueryHandler<GetAllVarietiesQuery, IReadOnlyList<VarietyDto>>
{
    public Task<Result<IReadOnlyList<VarietyDto>>> HandleAsync(
        GetAllVarietiesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var varieties = Variety
            .All.Select(v => new VarietyDto(v.Id, v.Name, v.Label, v.ShortName, v.Description))
            .ToList();

        return Task.FromResult(Result.Success<IReadOnlyList<VarietyDto>>(varieties));
    }
}
