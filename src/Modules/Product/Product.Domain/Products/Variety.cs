using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Variety : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Code { get; }

    private Variety(int id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    // --- Catalog entries (fill in your real values) ---
    public static readonly Variety Hass = new(1, "Hass", "HAS");

    public static IReadOnlyList<Variety> All => [Hass];

    public static Variety FromId(int id) =>
        All.FirstOrDefault(v => v.Id == id)
        ?? throw new InvalidOperationException($"Unknown Variety id: {id}");

    public static Variety FromCode(string code) =>
        All.FirstOrDefault(v => v.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown Variety code: {code}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
