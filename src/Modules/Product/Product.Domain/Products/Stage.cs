using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Stage : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Code { get; }

    private Stage(int id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    // --- Catalog entries (fill in your real values) ---
    public static readonly Stage Bulk = new(1, "Bulk", "BLK");

    public static IReadOnlyList<Stage> All => [Bulk];

    public static Stage FromId(int id) =>
        All.FirstOrDefault(s => s.Id == id)
        ?? throw new InvalidOperationException($"Unknown Stage id: {id}");

    public static Stage FromCode(string code) =>
        All.FirstOrDefault(s => s.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown Stage code: {code}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
