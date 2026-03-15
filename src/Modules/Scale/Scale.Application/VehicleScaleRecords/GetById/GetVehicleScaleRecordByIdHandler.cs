using LimonikOne.Modules.Scale.Application.VehicleScaleRecords;
using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.GetById;

internal sealed class GetVehicleScaleRecordByIdHandler
    : IQueryHandler<GetVehicleScaleRecordByIdQuery, VehicleScaleRecordDto>
{
    private readonly IVehicleScaleRecordRepository _repository;

    public GetVehicleScaleRecordByIdHandler(IVehicleScaleRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<VehicleScaleRecordDto>> HandleAsync(
        GetVehicleScaleRecordByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var record = await _repository.GetByIdAsync(
            VehicleScaleRecordId.From(query.Id),
            cancellationToken
        );

        if (record is null)
        {
            return Result.Failure<VehicleScaleRecordDto>(
                VehicleScaleRecordErrors.NotFound(query.Id)
            );
        }

        return Result.Success(VehicleScaleRecordDto.FromEntity(record));
    }
}
