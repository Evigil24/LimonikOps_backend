namespace LimonikOne.Modules.Person.Domain.Vendors;

public readonly record struct VendorId(Guid Value)
{
    public static VendorId New() => new(Guid.CreateVersion7());

    public static VendorId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
