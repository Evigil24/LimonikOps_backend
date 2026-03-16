namespace LimonikOne.Modules.Person.Domain.Customers;

public readonly record struct CustomerId(Guid Value)
{
    public static CustomerId New() => new(Guid.CreateVersion7());

    public static CustomerId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
