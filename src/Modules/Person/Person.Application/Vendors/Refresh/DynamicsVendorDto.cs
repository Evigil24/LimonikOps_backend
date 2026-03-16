namespace LimonikOne.Modules.Person.Application.Vendors.Refresh;

public sealed record DynamicsVendorDto(
    string VendorAccountNumber,
    string VendorGroupId,
    string VendorPartyNumber,
    string VendorSearchName,
    string RFCFederalTaxNumber,
    string VendorOrganizationName
);
