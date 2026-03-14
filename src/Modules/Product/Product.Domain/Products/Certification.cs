using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Certification : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Code { get; }

    private Certification(int id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    // --- Catalog entries (fill in your real values) ---
    public static readonly Certification Conventional = new(1, "Conventional", "CON");

    public static IReadOnlyList<Certification> All => [Conventional];

    public static Certification FromId(int id) =>
        All.FirstOrDefault(c => c.Id == id)
        ?? throw new InvalidOperationException($"Unknown Certification id: {id}");

    public static Certification FromCode(string code) =>
        All.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown Certification code: {code}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
