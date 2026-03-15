using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Person.Application.VendorClassifications.GetAll;

public sealed record GetAllVendorClassificationsQuery
    : IQuery<IReadOnlyList<VendorClassificationDto>>;
