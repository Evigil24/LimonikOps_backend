using LimonikOne.Modules.Scale.Application.VehicleScaleRecords;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.GetAll;

public sealed record GetAllVehicleScaleRecordsQuery : IQuery<IReadOnlyList<VehicleScaleRecordDto>>;
