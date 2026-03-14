using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Variety : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Label { get; }
    public string? ShortName { get; }
    public string? Description { get; }
    public int DisplayAttributesId { get; }

    private Variety(
        int id,
        string name,
        string label,
        string? shortName,
        string? description,
        int displayAttributesId
    )
    {
        Id = id;
        Name = name;
        Label = label;
        ShortName = shortName;
        Description = description;
        DisplayAttributesId = displayAttributesId;
    }

    public static readonly Variety Persian = new(1, "Persian", "Persa", "PR", "Limón persa", 1);

    public static readonly Variety Mexican = new(
        2,
        "Mexican",
        "Mexicano",
        "MX",
        "Limón mexicano",
        1
    );

    public static IReadOnlyList<Variety> All => [Persian, Mexican];

    public static Variety FromId(int id) =>
        All.FirstOrDefault(v => v.Id == id)
        ?? throw new InvalidOperationException($"Unknown Variety id: {id}");

    public static Variety FromName(string name) =>
        All.FirstOrDefault(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown Variety name: {name}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
