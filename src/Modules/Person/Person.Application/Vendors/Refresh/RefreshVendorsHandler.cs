using LimonikOne.Modules.Person.Domain.Vendors;
using LimonikOne.Shared.Abstractions.Application;
using LimonikOne.Shared.Abstractions.Dynamics;

namespace LimonikOne.Modules.Person.Application.Vendors.Refresh;

internal sealed class RefreshVendorsHandler(
    IDynamicsHttpClient dynamicsClient,
    IVendorRepository vendorRepository,
    IPersonUnitOfWork unitOfWork
) : ICommandHandler<RefreshVendorsCommand>
{
    private const string EntitySet = "VendorsV3";
    private const string SelectFields =
        "VendorAccountNumber,VendorGroupId,VendorPartyNumber,VendorSearchName,RFCFederalTaxNumber,VendorOrganizationName";

    private readonly IDynamicsHttpClient _dynamicsClient = dynamicsClient;
    private readonly IVendorRepository _vendorRepository = vendorRepository;
    private readonly IPersonUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(
        RefreshVendorsCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var dynamicsVendors = await _dynamicsClient.GetAsync<DynamicsVendorDto>(
            EntitySet,
            select: SelectFields,
            cancellationToken: cancellationToken
        );

        var existingVendors = await _vendorRepository.GetAllAsync(cancellationToken);
        var existingByAccountNumber = existingVendors.ToDictionary(v => v.AccountNumber);

        var newVendors = new List<VendorEntity>();

        foreach (var dv in dynamicsVendors)
        {
            if (existingByAccountNumber.TryGetValue(dv.VendorAccountNumber, out var existing))
            {
                existing.UpdateFromDynamics(
                    dv.VendorGroupId,
                    dv.VendorOrganizationName,
                    dv.VendorSearchName,
                    dv.VendorPartyNumber,
                    dv.RFCFederalTaxNumber
                );
            }
            else
            {
                newVendors.Add(
                    VendorEntity.CreateFromDynamics(
                        dv.VendorAccountNumber,
                        dv.VendorGroupId,
                        dv.VendorOrganizationName,
                        dv.VendorSearchName,
                        dv.VendorPartyNumber,
                        dv.RFCFederalTaxNumber
                    )
                );
            }
        }

        if (newVendors.Count > 0)
        {
            await _vendorRepository.AddRangeAsync(newVendors, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
