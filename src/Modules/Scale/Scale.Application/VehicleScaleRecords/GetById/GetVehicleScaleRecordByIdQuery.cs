using LimonikOne.Modules.Scale.Application.VehicleScaleRecords;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.GetById;

public sealed record GetVehicleScaleRecordByIdQuery(Guid Id) : IQuery<VehicleScaleRecordDto>;
