using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Certification : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Label { get; }
    public string? ShortName { get; }
    public string? Description { get; }
    public int DisplayAttributesId { get; }

    private Certification(
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

    public static readonly Certification Without = new(
        1,
        "Without",
        "Sin certificaciones",
        "00",
        null,
        1
    );

    public static readonly Certification FairTrade = new(
        2,
        "FairTrade",
        "Fair Trade",
        "FT",
        null,
        1
    );

    public static IReadOnlyList<Certification> All => [Without, FairTrade];

    public static Certification FromId(int id) =>
        All.FirstOrDefault(c => c.Id == id)
        ?? throw new InvalidOperationException($"Unknown Certification id: {id}");

    public static Certification FromName(string name) =>
        All.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown Certification name: {name}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
