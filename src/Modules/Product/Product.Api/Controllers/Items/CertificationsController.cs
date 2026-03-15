using LimonikOne.Modules.Product.Application.Items.Lookups.Certifications.GetAll;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Product.Api.Controllers.Items;

[ApiController]
[Tags("Item")]
[Route("api/Item/certifications")]
public sealed class CertificationsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CertificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices]
            IQueryHandler<GetAllCertificationsQuery, IReadOnlyList<CertificationDto>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetAllCertificationsQuery(), cancellationToken);

        return Ok(result.Value);
    }
}
