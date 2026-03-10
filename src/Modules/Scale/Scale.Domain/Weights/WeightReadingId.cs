namespace LimonikOne.Modules.Scale.Domain.Weights;

public readonly record struct WeightReadingId(Guid Value)
{
    public static WeightReadingId New() => new(Guid.CreateVersion7());

    public static WeightReadingId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
