namespace LimonikOne.Modules.Scale.Domain.WeightEvents;

public readonly record struct WeightEventId(Guid Value)
{
    public static WeightEventId New() => new(Guid.CreateVersion7());

    public static WeightEventId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
