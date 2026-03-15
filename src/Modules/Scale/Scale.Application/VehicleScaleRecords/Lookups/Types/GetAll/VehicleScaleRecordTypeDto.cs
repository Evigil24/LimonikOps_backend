namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Lookups.Types.GetAll;

public sealed record VehicleScaleRecordTypeDto(
    int Id,
    string Name,
    string Label,
    string? ShortName,
    string? Description
);
