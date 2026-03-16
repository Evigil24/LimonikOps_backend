using LimonikOne.Modules.Person.Domain.Vendors;

namespace LimonikOne.Modules.Person.Application.Vendors;

public sealed record VendorDto(
    Guid Id,
    string AccountNumber,
    string GroupId,
    Guid? ClassificationId,
    string Name,
    string SearchName,
    string PartyNumber,
    string RFCFederalTaxNumber
)
{
    public static VendorDto FromEntity(VendorEntity vendor)
    {
        return new VendorDto(
            vendor.Id.Value,
            vendor.AccountNumber,
            vendor.GroupId,
            vendor.ClassificationId?.Value,
            vendor.Name,
            vendor.SearchName,
            vendor.PartyNumber,
            vendor.RFCFederalTaxNumber
        );
    }
}
