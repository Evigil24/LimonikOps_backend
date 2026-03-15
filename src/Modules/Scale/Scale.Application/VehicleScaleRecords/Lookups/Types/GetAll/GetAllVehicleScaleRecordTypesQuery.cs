using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Lookups.Types.GetAll;

public sealed record GetAllVehicleScaleRecordTypesQuery
    : IQuery<IReadOnlyList<VehicleScaleRecordTypeDto>>;
