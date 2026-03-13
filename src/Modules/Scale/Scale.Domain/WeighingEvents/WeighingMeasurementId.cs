namespace LimonikOne.Modules.Scale.Domain.WeighingEvents;

public readonly record struct WeighingMeasurementId(Guid Value)
{
    public static WeighingMeasurementId New() => new(Guid.CreateVersion7());

    public static WeighingMeasurementId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
