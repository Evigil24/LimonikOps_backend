using LimonikOne.Modules.Person.Domain.VendorClassifications;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.VendorClassifications.GetById;

internal sealed class GetVendorClassificationByIdHandler
    : IQueryHandler<GetVendorClassificationByIdQuery, VendorClassificationDto>
{
    private readonly IVendorClassificationRepository _repository;

    public GetVendorClassificationByIdHandler(IVendorClassificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<VendorClassificationDto>> HandleAsync(
        GetVendorClassificationByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var classification = await _repository.GetByIdAsync(
            VendorClassificationId.From(query.Id),
            cancellationToken
        );

        if (classification is null)
        {
            return Result.Failure<VendorClassificationDto>(
                VendorClassificationErrors.NotFound(query.Id)
            );
        }

        return Result.Success(VendorClassificationDto.FromEntity(classification));
    }
}
