using LimonikOne.Modules.Product.Application.Items.Lookups.Varieties.GetAll;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Product.Api.Controllers.Items;

[ApiController]
[Tags("Item")]
[Route("api/Item/varieties")]
public sealed class VarietiesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VarietyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetAllVarietiesQuery, IReadOnlyList<VarietyDto>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetAllVarietiesQuery(), cancellationToken);

        return Ok(result.Value);
    }
}
