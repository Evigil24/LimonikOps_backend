using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.Vendors.GetAll;

public sealed record GetAllVendorsQuery : IQuery<IReadOnlyList<VendorDto>>;
