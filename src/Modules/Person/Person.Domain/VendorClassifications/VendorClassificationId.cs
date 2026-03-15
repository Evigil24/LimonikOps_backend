namespace LimonikOne.Modules.Person.Domain.VendorClassifications;

public readonly record struct VendorClassificationId(Guid Value)
{
    public static VendorClassificationId New() => new(Guid.CreateVersion7());

    public static VendorClassificationId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
