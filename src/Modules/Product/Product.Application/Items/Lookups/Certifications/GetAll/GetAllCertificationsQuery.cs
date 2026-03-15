using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.Lookups.Certifications.GetAll;

public sealed record GetAllCertificationsQuery : IQuery<IReadOnlyList<CertificationDto>>;
