using LimonikOne.Modules.Person.Domain.VendorClassifications;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.VendorClassifications.GetAll;

internal sealed class GetAllVendorClassificationsHandler
    : IQueryHandler<GetAllVendorClassificationsQuery, IReadOnlyList<VendorClassificationDto>>
{
    private readonly IVendorClassificationRepository _repository;

    public GetAllVendorClassificationsHandler(IVendorClassificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<VendorClassificationDto>>> HandleAsync(
        GetAllVendorClassificationsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var classifications = await _repository.GetAllAsync(cancellationToken);

        var dtos = classifications.Select(VendorClassificationDto.FromEntity).ToList();

        return Result.Success<IReadOnlyList<VendorClassificationDto>>(dtos);
    }
}
