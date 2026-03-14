using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products;

internal static class ProductErrors
{
    public static Error DuplicateItemNumber(string itemNumber) =>
        Error.Conflict(
            "Product.DuplicateItemNumber",
            $"A product with item number '{itemNumber}' already exists."
        );

    public static Error NotFound(Guid id) =>
        Error.NotFound("Product.NotFound", $"Product with id '{id}' was not found.");
}
