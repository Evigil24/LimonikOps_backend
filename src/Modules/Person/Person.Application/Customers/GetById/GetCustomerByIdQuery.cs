using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Customers.GetById;

public sealed record GetCustomerByIdQuery(Guid Id) : IQuery<CustomerDto>;
