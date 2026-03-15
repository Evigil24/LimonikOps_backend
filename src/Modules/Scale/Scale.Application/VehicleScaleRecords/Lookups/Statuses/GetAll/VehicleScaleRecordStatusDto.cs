namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Lookups.Statuses.GetAll;

public sealed record VehicleScaleRecordStatusDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
