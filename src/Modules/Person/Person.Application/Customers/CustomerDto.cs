using LimonikOne.Modules.Person.Domain.Customers;

namespace LimonikOne.Modules.Person.Application.Customers;

public sealed record CustomerDto(
    Guid Id,
    string AccountNumber,
    string GroupId,
    string Name,
    string SearchName,
    string PartyNumber,
    string RFCFederalTaxNumber
)
{
    public static CustomerDto FromEntity(CustomerEntity customer)
    {
        return new CustomerDto(
            customer.Id.Value,
            customer.AccountNumber,
            customer.GroupId,
            customer.Name,
            customer.SearchName,
            customer.PartyNumber,
            customer.RFCFederalTaxNumber
        );
    }
}
