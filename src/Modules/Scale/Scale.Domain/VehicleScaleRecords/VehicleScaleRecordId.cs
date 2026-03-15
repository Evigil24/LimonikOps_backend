namespace LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;

public readonly record struct VehicleScaleRecordId(Guid Value)
{
    public static VehicleScaleRecordId New() => new(Guid.CreateVersion7());

    public static VehicleScaleRecordId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
