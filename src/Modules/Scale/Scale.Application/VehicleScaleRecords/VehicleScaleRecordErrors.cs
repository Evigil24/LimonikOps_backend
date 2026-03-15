using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords;

internal static class VehicleScaleRecordErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "VehicleScaleRecord.NotFound",
            $"Vehicle scale record with id '{id}' was not found."
        );
}
