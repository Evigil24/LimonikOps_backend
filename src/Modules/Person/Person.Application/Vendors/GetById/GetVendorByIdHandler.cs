using LimonikOne.Modules.Person.Domain.Vendors;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Vendors.GetById;

internal sealed class GetVendorByIdHandler : IQueryHandler<GetVendorByIdQuery, VendorDto>
{
    private readonly IVendorRepository _repository;

    public GetVendorByIdHandler(IVendorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<VendorDto>> HandleAsync(
        GetVendorByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var vendor = await _repository.GetByIdAsync(VendorId.From(query.Id), cancellationToken);

        if (vendor is null)
        {
            return Result.Failure<VendorDto>(VendorErrors.NotFound(query.Id));
        }

        return Result.Success(VendorDto.FromEntity(vendor));
    }
}
