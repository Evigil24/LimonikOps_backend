using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords;

public sealed record VehicleScaleRecordDto(
    Guid Id,
    int TypeId,
    string TypeName,
    string TypeLabel,
    int StatusId,
    string StatusName,
    string StatusLabel,
    DateTime StartedAt,
    DateTime? ClosedAt,
    decimal? FirstWeight,
    Guid? FirstWeightId,
    decimal? SecondWeight,
    Guid? SecondWeightId,
    decimal? NetWeight,
    DateTime CreatedAt,
    string CreatedBy
)
{
    public static VehicleScaleRecordDto FromEntity(VehicleScaleRecordEntity record)
    {
        return new VehicleScaleRecordDto(
            record.Id.Value,
            record.Type.Id,
            record.Type.Name,
            record.Type.Label,
            record.Status.Id,
            record.Status.Name,
            record.Status.Label,
            record.StartedAt,
            record.ClosedAt,
            record.FirstWeight,
            record.FirstWeightId?.Value,
            record.SecondWeight,
            record.SecondWeightId?.Value,
            record.NetWeight,
            record.CreatedAt,
            record.CreatedBy
        );
    }
}
