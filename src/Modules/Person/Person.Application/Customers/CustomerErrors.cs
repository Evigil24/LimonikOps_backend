using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Customers;

internal static class CustomerErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Customer.NotFound", $"Customer with id '{id}' was not found.");
}
