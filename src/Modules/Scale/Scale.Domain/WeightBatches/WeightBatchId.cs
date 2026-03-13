namespace LimonikOne.Modules.Scale.Domain.WeightBatches;

public readonly record struct WeightBatchId(Guid Value)
{
    public static WeightBatchId New() => new(Guid.CreateVersion7());

    public static WeightBatchId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
