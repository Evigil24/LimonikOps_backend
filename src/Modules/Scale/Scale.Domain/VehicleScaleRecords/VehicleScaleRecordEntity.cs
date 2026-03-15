using LimonikOne.Modules.Scale.Domain.WeightReadings;
using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;

public sealed class VehicleScaleRecordEntity : AggregateRoot<VehicleScaleRecordId>
{
    public VehicleScaleRecordType Type { get; private set; } = null!;
    public VehicleScaleRecordStatus Status { get; private set; } = null!;
    public DateTime StartedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public decimal? FirstWeight { get; private set; }
    public WeightReadingId? FirstWeightId { get; private set; }
    public decimal? SecondWeight { get; private set; }
    public WeightReadingId? SecondWeightId { get; private set; }
    public decimal? NetWeight { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; } = null!;

    private VehicleScaleRecordEntity() { } // EF Core

    public static VehicleScaleRecordEntity Create(VehicleScaleRecordType type, string createdBy)
    {
        return new VehicleScaleRecordEntity
        {
            Id = VehicleScaleRecordId.New(),
            Type = type,
            Status = VehicleScaleRecordStatus.InProgress,
            StartedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }
}
