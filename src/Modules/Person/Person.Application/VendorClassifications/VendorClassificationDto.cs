using LimonikOne.Modules.Person.Domain.VendorClassifications;

namespace LimonikOne.Modules.Person.Application.VendorClassifications;

public sealed record VendorClassificationDto(
    Guid Id,
    string Name,
    string? Description,
    Guid? ParentId
)
{
    public static VendorClassificationDto FromEntity(VendorClassificationEntity classification)
    {
        return new VendorClassificationDto(
            classification.Id.Value,
            classification.Name,
            classification.Description,
            classification.ParentId?.Value
        );
    }
}
