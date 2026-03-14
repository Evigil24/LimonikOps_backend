using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Handling : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Code { get; }

    private Handling(int id, string name, string code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    // --- Catalog entries (fill in your real values) ---
    public static readonly Handling Fresh = new(1, "Fresh", "FRS");

    public static IReadOnlyList<Handling> All => [Fresh];

    public static Handling FromId(int id) =>
        All.FirstOrDefault(h => h.Id == id)
        ?? throw new InvalidOperationException($"Unknown Handling id: {id}");

    public static Handling FromCode(string code) =>
        All.FirstOrDefault(h => h.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown Handling code: {code}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
