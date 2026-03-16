using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Customers.GetAll;

public sealed record GetAllCustomersQuery : IQuery<IReadOnlyList<CustomerDto>>;
