namespace LimonikOne.Modules.Reception.Domain.Receptions;

public readonly record struct ReceptionId(Guid Value)
{
    public static ReceptionId New() => new(Guid.CreateVersion7());
    public static ReceptionId From(Guid value) => new(value);
    public override string ToString() => Value.ToString();
}
