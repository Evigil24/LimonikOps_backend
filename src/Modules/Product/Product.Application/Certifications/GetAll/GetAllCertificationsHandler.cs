using LimonikOne.Modules.Product.Domain.Products;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Certifications.GetAll;

internal sealed class GetAllCertificationsHandler
    : IQueryHandler<GetAllCertificationsQuery, IReadOnlyList<CertificationDto>>
{
    public Task<Result<IReadOnlyList<CertificationDto>>> HandleAsync(
        GetAllCertificationsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var certifications = Certification
            .All.Select(c => new CertificationDto(
                c.Id,
                c.Name,
                c.Label,
                c.ShortName,
                c.Description
            ))
            .ToList();

        return Task.FromResult(Result.Success<IReadOnlyList<CertificationDto>>(certifications));
    }
}
