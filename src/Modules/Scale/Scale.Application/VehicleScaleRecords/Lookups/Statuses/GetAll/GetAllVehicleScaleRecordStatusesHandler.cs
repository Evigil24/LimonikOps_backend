using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Lookups.Statuses.GetAll;

internal sealed class GetAllVehicleScaleRecordStatusesHandler
    : IQueryHandler<GetAllVehicleScaleRecordStatusesQuery, IReadOnlyList<VehicleScaleRecordStatusDto>>
{
    public Task<Result<IReadOnlyList<VehicleScaleRecordStatusDto>>> HandleAsync(
        GetAllVehicleScaleRecordStatusesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var statuses = VehicleScaleRecordStatus
            .All.Select(status => new VehicleScaleRecordStatusDto(
                status.Id,
                status.Name,
                status.Label,
                status.ShortName,
                status.Description
            ))
            .ToList();

        return Task.FromResult(Result.Success<IReadOnlyList<VehicleScaleRecordStatusDto>>(statuses));
    }
}
