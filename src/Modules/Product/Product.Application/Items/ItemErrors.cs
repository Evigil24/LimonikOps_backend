using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items;

internal static class ItemErrors
{
    public static Error DuplicateItemNumber(string itemNumber) =>
        Error.Conflict(
            "Item.DuplicateItemNumber",
            $"An item with item number '{itemNumber}' already exists."
        );

    public static Error NotFound(Guid id) =>
        Error.NotFound("Item.NotFound", $"Item with id '{id}' was not found.");
}
