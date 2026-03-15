using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Lookups.Types.GetAll;

internal sealed class GetAllVehicleScaleRecordTypesHandler
    : IQueryHandler<GetAllVehicleScaleRecordTypesQuery, IReadOnlyList<VehicleScaleRecordTypeDto>>
{
    public Task<Result<IReadOnlyList<VehicleScaleRecordTypeDto>>> HandleAsync(
        GetAllVehicleScaleRecordTypesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var types = VehicleScaleRecordType
            .All.Select(type => new VehicleScaleRecordTypeDto(
                type.Id,
                type.Name,
                type.Label,
                type.ShortName,
                type.Description
            ))
            .ToList();

        return Task.FromResult(Result.Success<IReadOnlyList<VehicleScaleRecordTypeDto>>(types));
    }
}
