using LimonikOne.Modules.Product.Application.Items;
using LimonikOne.Modules.Product.Domain.Items;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.GetAll;

internal sealed class GetAllItemsHandler(IItemRepository itemRepository)
    : IQueryHandler<GetAllItemsQuery, IReadOnlyList<ItemDto>>
{
    private readonly IItemRepository _itemRepository = itemRepository;

    public async Task<Result<IReadOnlyList<ItemDto>>> HandleAsync(
        GetAllItemsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var items = await _itemRepository.GetAllAsync(cancellationToken);

        var dtos = items.Select(ItemDto.FromEntity).ToList();

        return Result.Success<IReadOnlyList<ItemDto>>(dtos);
    }
}
