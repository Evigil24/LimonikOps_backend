namespace LimonikOne.Modules.Scale.Domain.WeightEvents;

public readonly record struct WeightMeasurementId(Guid Value)
{
    public static WeightMeasurementId New() => new(Guid.CreateVersion7());

    public static WeightMeasurementId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
