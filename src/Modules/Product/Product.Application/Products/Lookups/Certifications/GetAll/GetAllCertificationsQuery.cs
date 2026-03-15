using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.Lookups.Certifications.GetAll;

public sealed record GetAllCertificationsQuery : IQuery<IReadOnlyList<CertificationDto>>;
