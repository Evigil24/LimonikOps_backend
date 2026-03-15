using LimonikOne.Modules.Person.Application.VendorClassifications;
using LimonikOne.Modules.Person.Application.VendorClassifications.GetAll;
using LimonikOne.Modules.Person.Application.VendorClassifications.GetById;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Person.Api.Controllers.VendorClassifications;

[ApiController]
[Tags("VendorClassification")]
[Route("api/vendor-classifications")]
public sealed class VendorClassificationsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VendorClassificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices]
            IQueryHandler<
            GetAllVendorClassificationsQuery,
            IReadOnlyList<VendorClassificationDto>
        > handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(
            new GetAllVendorClassificationsQuery(),
            cancellationToken
        );

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VendorClassificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices]
            IQueryHandler<GetVendorClassificationByIdQuery, VendorClassificationDto> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(
            new GetVendorClassificationByIdQuery(id),
            cancellationToken
        );

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error!.Message,
                statusCode: StatusCodes.Status404NotFound,
                title: result.Error.Code
            );
        }

        return Ok(result.Value);
    }
}
