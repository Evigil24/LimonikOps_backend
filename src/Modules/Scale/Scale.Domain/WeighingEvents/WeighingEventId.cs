namespace LimonikOne.Modules.Scale.Domain.WeighingEvents;

public readonly record struct WeighingEventId(Guid Value)
{
    public static WeighingEventId New() => new(Guid.CreateVersion7());

    public static WeighingEventId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
