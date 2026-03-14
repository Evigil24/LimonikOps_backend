namespace LimonikOne.Modules.Product.Domain.Products;

public readonly record struct ProductId(Guid Value)
{
    public static ProductId New() => new(Guid.CreateVersion7());

    public static ProductId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
