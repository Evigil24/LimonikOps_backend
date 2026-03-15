using LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Lookups.Statuses.GetAll;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Scale.Api.Controllers.VehicleScaleRecords;

[ApiController]
[Tags("Vehicle Scale Record")]
[Route("api/vehicle-scale-records/statuses")]
public sealed class VehicleScaleRecordStatusesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VehicleScaleRecordStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices]
            IQueryHandler<
                GetAllVehicleScaleRecordStatusesQuery,
                IReadOnlyList<VehicleScaleRecordStatusDto>
            > handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(
            new GetAllVehicleScaleRecordStatusesQuery(),
            cancellationToken
        );

        return Ok(result.Value);
    }
}
