using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Items;

public sealed class Item : AggregateRoot<ItemId>
{
    public string ItemNumber { get; private set; } = null!;
    public string PrimaryName { get; private set; } = null!;
    public string SearchName { get; private set; } = null!;
    public Variety Variety { get; private set; } = null!;
    public Handling Handling { get; private set; } = null!;
    public Certification Certification { get; private set; } = null!;
    public Stage Stage { get; private set; } = null!;

    private Item() { } // EF Core

    public static Item Create(
        string itemNumber,
        string primaryName,
        string searchName,
        Variety variety,
        Handling handling,
        Certification certification,
        Stage stage
    )
    {
        return new Item
        {
            Id = ItemId.New(),
            ItemNumber = itemNumber,
            PrimaryName = primaryName,
            SearchName = searchName,
            Variety = variety,
            Handling = handling,
            Certification = certification,
            Stage = stage,
        };
    }
}
