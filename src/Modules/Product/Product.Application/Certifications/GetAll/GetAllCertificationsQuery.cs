using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Certifications.GetAll;

public sealed record GetAllCertificationsQuery : IQuery<IReadOnlyList<CertificationDto>>;
