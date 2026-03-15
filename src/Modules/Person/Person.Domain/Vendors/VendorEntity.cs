using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Person.Domain.Vendors;

public sealed class VendorEntity : AggregateRoot<VendorId>
{
    public string AccountNumber { get; private set; } = null!;
    public string GroupId { get; private set; } = null!;
    public int ClassificationId { get; private set; }
    public string Name { get; private set; } = null!;
    public string SearchName { get; private set; } = null!;
    public string PartyNumber { get; private set; } = null!;
    public string RFCFederalTaxNumber { get; private set; } = null!;

    private VendorEntity() { }

    public static VendorEntity Create(
        string accountNumber,
        string groupId,
        int classificationId,
        string name,
        string searchName,
        string partyNumber,
        string rfcFederalTaxNumber
    )
    {
        return new VendorEntity
        {
            Id = VendorId.New(),
            AccountNumber = accountNumber,
            GroupId = groupId,
            ClassificationId = classificationId,
            Name = name,
            SearchName = searchName,
            PartyNumber = partyNumber,
            RFCFederalTaxNumber = rfcFederalTaxNumber,
        };
    }
}
