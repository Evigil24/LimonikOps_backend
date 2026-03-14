using LimonikOne.Modules.Product.Application.Handlings.GetAll;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Product.Api.Controllers;

[ApiController]
[Route("api/product-handlings")]
public sealed class HandlingsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<HandlingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetAllHandlingsQuery, IReadOnlyList<HandlingDto>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetAllHandlingsQuery(), cancellationToken);

        return Ok(result.Value);
    }
}
