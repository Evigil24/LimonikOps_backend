using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Vendors;

internal static class VendorErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Vendor.NotFound", $"Vendor with id '{id}' was not found.");
}
