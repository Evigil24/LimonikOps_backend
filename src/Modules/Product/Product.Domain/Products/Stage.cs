using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Product.Domain.Products;

public sealed class Stage : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Label { get; }
    public string? ShortName { get; }
    public string? Description { get; }
    public int DisplayAttributesId { get; }

    private Stage(
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

    public static readonly Stage Bulk = new(
        1,
        "Bulk",
        "Granel",
        "GR",
        "Limón a granel sin clasificar",
        1
    );

    public static readonly Stage Sorted = new(
        2,
        "Sorted",
        "Clasificado",
        "CL",
        "Limón a procesado ya clasificado",
        1
    );

    public static readonly Stage Finished = new(
        3,
        "Finished",
        "Producto terminado",
        "PT",
        "Productos terminados",
        1
    );

    public static readonly Stage Byproduct = new(
        4,
        "Byproduct",
        "Coproducto",
        "CO",
        "Productos secundarios obtenidos en el proceso",
        1
    );

    public static IReadOnlyList<Stage> All => [Bulk, Sorted, Finished, Byproduct];

    public static Stage FromId(int id) =>
        All.FirstOrDefault(s => s.Id == id)
        ?? throw new InvalidOperationException($"Unknown Stage id: {id}");

    public static Stage FromName(string name) =>
        All.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown Stage name: {name}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
