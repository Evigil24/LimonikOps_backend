using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Create;

internal sealed class CreateVehicleScaleRecordHandler
    : ICommandHandler<CreateVehicleScaleRecordCommand, Guid>
{
    private readonly IVehicleScaleRecordRepository _repository;
    private readonly IScaleUnitOfWork _unitOfWork;

    public CreateVehicleScaleRecordHandler(
        IVehicleScaleRecordRepository repository,
        IScaleUnitOfWork unitOfWork
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> HandleAsync(
        CreateVehicleScaleRecordCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var record = VehicleScaleRecordEntity.Create(
            VehicleScaleRecordType.FromId(command.TypeId),
            command.CreatedBy
        );

        await _repository.AddAsync(record, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(record.Id.Value);
    }
}
