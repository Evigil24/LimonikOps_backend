using LimonikOne.Modules.Product.Domain.Items;

namespace LimonikOne.Modules.Product.Application.Items;

public sealed record ItemDto(
    Guid Id,
    long DisplayId,
    string ItemNumber,
    string PrimaryName,
    string SearchName,
    string Variety,
    string Handling,
    string Certification,
    string Stage
)
{
    public static ItemDto FromEntity(Item item)
    {
        return new ItemDto(
            item.Id.Value,
            item.DisplayId,
            item.ItemNumber,
            item.PrimaryName,
            item.SearchName,
            item.Variety.Label,
            item.Handling.Label,
            item.Certification.Label,
            item.Stage.Label
        );
    }
}
