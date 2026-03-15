using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;

public sealed class VehicleScaleRecordStatus : ValueObject
{
    public int Id { get; }
    public string Name { get; }
    public string Label { get; }
    public string? ShortName { get; }
    public string? Description { get; }
    public int DisplayAttributesId { get; }

    private VehicleScaleRecordStatus(
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

    public static readonly VehicleScaleRecordStatus InProgress = new(
        1,
        "InProgress",
        "En progreso",
        "EP",
        "Registro en progreso",
        1
    );
    public static readonly VehicleScaleRecordStatus Completed = new(
        2,
        "Completed",
        "Completado",
        "CO",
        "Registro completado",
        1
    );
    public static readonly VehicleScaleRecordStatus Cancelled = new(
        3,
        "Cancelled",
        "Cancelado",
        "CA",
        "Registro cancelado",
        1
    );

    public static IReadOnlyList<VehicleScaleRecordStatus> All => [InProgress, Completed, Cancelled];

    public static VehicleScaleRecordStatus FromId(int id) =>
        All.FirstOrDefault(s => s.Id == id)
        ?? throw new InvalidOperationException($"Unknown VehicleScaleRecordStatus id: {id}");

    public static VehicleScaleRecordStatus FromName(string name) =>
        All.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unknown VehicleScaleRecordStatus name: {name}");

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
    }
}
