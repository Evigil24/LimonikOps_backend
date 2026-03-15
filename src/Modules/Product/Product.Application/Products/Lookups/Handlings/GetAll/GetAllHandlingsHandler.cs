using LimonikOne.Modules.Product.Domain.Products;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.Lookups.Handlings.GetAll;

internal sealed class GetAllHandlingsHandler
    : IQueryHandler<GetAllHandlingsQuery, IReadOnlyList<HandlingDto>>
{
    public Task<Result<IReadOnlyList<HandlingDto>>> HandleAsync(
        GetAllHandlingsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var handlings = Handling
            .All.Select(h => new HandlingDto(h.Id, h.Name, h.Label, h.ShortName, h.Description))
            .ToList();

        return Task.FromResult(Result.Success<IReadOnlyList<HandlingDto>>(handlings));
    }
}
