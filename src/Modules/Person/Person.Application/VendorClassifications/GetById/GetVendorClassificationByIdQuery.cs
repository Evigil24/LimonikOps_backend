using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.VendorClassifications.GetById;

public sealed record GetVendorClassificationByIdQuery(Guid Id) : IQuery<VendorClassificationDto>;
