using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Person.Domain.Customers;

public sealed class CustomerEntity : AggregateRoot<CustomerId>
{
    public string AccountNumber { get; private set; } = null!;
    public string GroupId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string SearchName { get; private set; } = null!;
    public string PartyNumber { get; private set; } = null!;
    public string RFCFederalTaxNumber { get; private set; } = null!;

    private CustomerEntity() { }

    public static CustomerEntity Create(
        string accountNumber,
        string groupId,
        string name,
        string searchName,
        string partyNumber,
        string rfcFederalTaxNumber
    )
    {
        return new CustomerEntity
        {
            Id = CustomerId.New(),
            AccountNumber = accountNumber,
            GroupId = groupId,
            Name = name,
            SearchName = searchName,
            PartyNumber = partyNumber,
            RFCFederalTaxNumber = rfcFederalTaxNumber,
        };
    }

    public static CustomerEntity CreateFromDynamics(
        string accountNumber,
        string groupId,
        string name,
        string searchName,
        string partyNumber,
        string rfcFederalTaxNumber
    )
    {
        return new CustomerEntity
        {
            Id = CustomerId.New(),
            AccountNumber = accountNumber,
            GroupId = groupId,
            Name = name,
            SearchName = searchName,
            PartyNumber = partyNumber,
            RFCFederalTaxNumber = rfcFederalTaxNumber,
        };
    }

    public void UpdateFromDynamics(
        string groupId,
        string name,
        string searchName,
        string partyNumber,
        string rfcFederalTaxNumber
    )
    {
        GroupId = groupId;
        Name = name;
        SearchName = searchName;
        PartyNumber = partyNumber;
        RFCFederalTaxNumber = rfcFederalTaxNumber;
    }
}
