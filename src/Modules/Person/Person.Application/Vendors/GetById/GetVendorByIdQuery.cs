using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Vendors.GetById;

public sealed record GetVendorByIdQuery(Guid Id) : IQuery<VendorDto>;
