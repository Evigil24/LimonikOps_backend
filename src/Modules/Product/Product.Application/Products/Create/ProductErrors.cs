using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.Create;

internal static class ProductErrors
{
    public static Error DuplicateItemNumber(string itemNumber) =>
        Error.Conflict(
            "Product.DuplicateItemNumber",
            $"A product with item number '{itemNumber}' already exists."
        );
}
