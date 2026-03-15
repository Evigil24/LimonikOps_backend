using FluentValidation;
using LimonikOne.Modules.Scale.Api.Controllers.VehicleScaleRecords.Requests;
using LimonikOne.Modules.Scale.Application.VehicleScaleRecords.Create;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Scale.Api.Controllers.VehicleScaleRecords;

[ApiController]
[Tags("Vehicle Scale Record")]
[Route("api/vehicle-scale-records")]
public sealed class VehicleScaleRecordsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateVehicleScaleRecordRequest request,
        [FromServices] IValidator<CreateVehicleScaleRecordCommand> validator,
        [FromServices] ICommandHandler<CreateVehicleScaleRecordCommand, Guid> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateVehicleScaleRecordCommand(request.TypeId, request.CreatedBy);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(
                new ValidationProblemDetails(
                    validationResult
                        .Errors.GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                )
            );
        }

        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error!.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: result.Error.Code
            );
        }

        return StatusCode(StatusCodes.Status201Created, new { id = result.Value });
    }
}
