using LimonikOne.Modules.Product.Application.Items;
using LimonikOne.Modules.Product.Domain.Items;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.GetById;

internal sealed class GetItemByIdHandler(IItemRepository itemRepository)
    : IQueryHandler<GetItemByIdQuery, ItemDto>
{
    private readonly IItemRepository _itemRepository = itemRepository;

    public async Task<Result<ItemDto>> HandleAsync(
        GetItemByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var item = await _itemRepository.GetByIdAsync(ItemId.From(query.Id), cancellationToken);

        if (item is null)
        {
            return Result.Failure<ItemDto>(ItemErrors.NotFound(query.Id));
        }

        return Result.Success(ItemDto.FromEntity(item));
    }
}
