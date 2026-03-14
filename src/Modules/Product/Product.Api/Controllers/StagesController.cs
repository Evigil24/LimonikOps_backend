using LimonikOne.Modules.Product.Application.Stages.GetAll;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Product.Api.Controllers;

[ApiController]
[Route("api/product-stages")]
public sealed class StagesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetAllStagesQuery, IReadOnlyList<StageDto>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetAllStagesQuery(), cancellationToken);

        return Ok(result.Value);
    }
}
