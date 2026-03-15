namespace LimonikOne.Modules.Product.Domain.Items;

public readonly record struct ItemId(Guid Value)
{
    public static ItemId New() => new(Guid.CreateVersion7());

    public static ItemId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
