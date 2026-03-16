using LimonikOne.Modules.Person.Application.Vendors;
using LimonikOne.Modules.Person.Application.Vendors.GetAll;
using LimonikOne.Modules.Person.Application.Vendors.GetById;
using LimonikOne.Modules.Person.Application.Vendors.Refresh;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Person.Api.Controllers.Vendors;

[ApiController]
[Tags("Vendor")]
[Route("api/vendors")]
public sealed class VendorsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VendorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetAllVendorsQuery, IReadOnlyList<VendorDto>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetAllVendorsQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VendorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetVendorByIdQuery, VendorDto> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetVendorByIdQuery(id), cancellationToken);

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

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh(
        [FromServices] ICommandHandler<RefreshVendorsCommand> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new RefreshVendorsCommand(), cancellationToken);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error!.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: result.Error.Code
            );
        }

        return Ok();
    }
}
