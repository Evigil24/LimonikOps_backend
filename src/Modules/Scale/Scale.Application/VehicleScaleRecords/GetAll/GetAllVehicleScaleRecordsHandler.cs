using LimonikOne.Modules.Scale.Application.VehicleScaleRecords;
using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.GetAll;

internal sealed class GetAllVehicleScaleRecordsHandler(IVehicleScaleRecordRepository repository)
    : IQueryHandler<GetAllVehicleScaleRecordsQuery, IReadOnlyList<VehicleScaleRecordDto>>
{
    private readonly IVehicleScaleRecordRepository _repository = repository;

    public async Task<Result<IReadOnlyList<VehicleScaleRecordDto>>> HandleAsync(
        GetAllVehicleScaleRecordsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var records = await _repository.GetAllAsync(cancellationToken);

        var dtos = records.Select(VehicleScaleRecordDto.FromEntity).ToList();

        return Result.Success<IReadOnlyList<VehicleScaleRecordDto>>(dtos);
    }
}
