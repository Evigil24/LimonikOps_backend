using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Create;

public sealed record CreateVehicleScaleRecordCommand(int TypeId, string CreatedBy) : ICommand<Guid>;
