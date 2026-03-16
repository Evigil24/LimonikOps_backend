namespace LimonikOne.Modules.Person.Application.Customers.Refresh;

public sealed record DynamicsCustomerDto(
    string CustomerAccount,
    string ItemCustomerGroupId,
    string PartyNumber,
    string NameAlias,
    string RFCNumber,
    string OrganizationName
);
