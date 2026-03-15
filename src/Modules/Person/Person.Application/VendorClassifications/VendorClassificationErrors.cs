using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.VendorClassifications;

internal static class VendorClassificationErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "VendorClassification.NotFound",
            $"Vendor classification with id '{id}' was not found."
        );
}
