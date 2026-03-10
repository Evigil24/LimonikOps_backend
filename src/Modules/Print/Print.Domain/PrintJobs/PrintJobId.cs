namespace LimonikOne.Modules.Print.Domain.PrintJobs;

public readonly record struct PrintJobId(Guid Value)
{
    public static PrintJobId New() => new(Guid.CreateVersion7());

    public static PrintJobId From(Guid value) => new(value);

    public override string ToString() => Value.ToString();
}
