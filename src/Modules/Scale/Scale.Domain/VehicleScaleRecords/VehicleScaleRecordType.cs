using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;

public sealed class VehicleScaleRecordType : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Label { get; }
    public string? ShortName { get; }
    public string? Description { get; }
    public int DisplayAttributesId { get; }

    private VehicleScaleRecordType(
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

    public static readonly VehicleScaleRecordType Inbound = new(
        1,
        "Inbound",
        "Entrada",
        "EN",
        "Registro de entrada de material",
        1
    );
    public static readonly VehicleScaleRecordType Outbound = new(
        2,
        "Outbound",
        "Salida",
        "SA",
        "Registro de salida de material",
        1
    );

    public static readonly VehicleScaleRecordType WeighIn = new(
        3,
        "Purchase",
        "Compra",
        "CO",
        "Registro de compra de mercancía",
        1
    );
    public static readonly VehicleScaleRecordType WeighOut = new(
        4,
        "Sale",
        "Venta",
        "VE",
        "Registro de venta de mercancía",
        1
    );

    public static IReadOnlyList<VehicleScaleRecordType> All =>
        [Inbound, Outbound, WeighIn, WeighOut];

    public static VehicleScaleRecordType FromId(int id) =>
        All.FirstOrDefault(t => t.Id == id)
        ?? throw new InvalidOperationException($"Unknown VehicleScaleRecordType id: {id}");

    public static VehicleScaleRecordType FromName(string name) =>
        All.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown VehicleScaleRecordType name: {name}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
