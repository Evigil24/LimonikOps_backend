using LimonikOne.Shared.Abstractions.Domain;

namespace LimonikOne.Modules.Person.Domain.VendorClassifications;

public sealed class VendorClassificationEntity : AggregateRoot<VendorClassificationId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public VendorClassificationId? ParentId { get; private set; }
    public VendorClassificationEntity? Parent { get; private set; }

    private readonly List<VendorClassificationEntity> _children = [];
    public IReadOnlyList<VendorClassificationEntity> Children => _children.AsReadOnly();

    private VendorClassificationEntity() { }

    public static VendorClassificationEntity Create(
        string name,
        string? description,
        VendorClassificationId? parentId
    )
    {
        return new VendorClassificationEntity
        {
            Id = VendorClassificationId.New(),
            Name = name,
            Description = description,
            ParentId = parentId,
        };
    }

    public void Update(string name, string? description, VendorClassificationId? parentId)
    {
        Name = name;
        Description = description;
        ParentId = parentId;
    }
}
