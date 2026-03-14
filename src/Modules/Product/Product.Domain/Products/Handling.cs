using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Handling : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Label { get; }
    public string? ShortName { get; }
    public string? Description { get; }
    public int DisplayAttributesId { get; }

    private Handling(
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

    public static readonly Handling Conventional = new(
        1,
        "Conventional",
        "Convencional",
        "CON",
        null,
        1
    );

    public static readonly Handling Organic = new(2, "Organic", "Orgánico", "ORG", null, 1);

    public static IReadOnlyList<Handling> All => [Conventional, Organic];

    public static Handling FromId(int id) =>
        All.FirstOrDefault(h => h.Id == id)
        ?? throw new InvalidOperationException($"Unknown Handling id: {id}");

    public static Handling FromName(string name) =>
        All.FirstOrDefault(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown Handling name: {name}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
