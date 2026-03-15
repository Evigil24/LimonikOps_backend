using LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Lookups.Types.GetAll;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Scale.Api.Controllers.VehicleScaleRecords;

[ApiController]
[Tags("Vehicle Scale Record")]
[Route("api/vehicle-scale-records/types")]
public sealed class VehicleScaleRecordTypesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VehicleScaleRecordTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices]
            IQueryHandler<GetAllVehicleScaleRecordTypesQuery, IReadOnlyList<VehicleScaleRecordTypeDto>>
                handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(
            new GetAllVehicleScaleRecordTypesQuery(),
            cancellationToken
        );

        return Ok(result.Value);
    }
}
