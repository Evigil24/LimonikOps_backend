using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Lookups.Statuses.GetAll;

public sealed record GetAllVehicleScaleRecordStatusesQuery
    : IQuery<IReadOnlyList<VehicleScaleRecordStatusDto>>;
