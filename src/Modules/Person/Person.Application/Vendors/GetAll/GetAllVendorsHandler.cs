using LimonikOne.Modules.Person.Domain.Vendors;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Vendors.GetAll;

internal sealed class GetAllVendorsHandler
    : IQueryHandler<GetAllVendorsQuery, IReadOnlyList<VendorDto>>
{
    private readonly IVendorRepository _repository;

    public GetAllVendorsHandler(IVendorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<VendorDto>>> HandleAsync(
        GetAllVendorsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var vendors = await _repository.GetAllAsync(cancellationToken);

        var dtos = vendors.Select(VendorDto.FromEntity).ToList();

        return Result.Success<IReadOnlyList<VendorDto>>(dtos);
    }
}
