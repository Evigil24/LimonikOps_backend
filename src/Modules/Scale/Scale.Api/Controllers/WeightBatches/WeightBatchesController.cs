using FluentValidation;
using LimonikOne.Modules.Scale.Api.Controllers.WeightBatches.Requests;
using LimonikOne.Modules.Scale.Api.Filters;
using LimonikOne.Modules.Scale.Application.WeightBatches.Ingest;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Scale.Api.Controllers.WeightBatches;

[ApiController]
[Route("api/weight-batches")]
public sealed class WeightBatchesController : ControllerBase
{
    [HttpPost]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Ingest(
        [FromBody] IngestWeightBatchRequest request,
        [FromServices] IValidator<IngestWeightBatchCommand> validator,
        [FromServices] ICommandHandler<IngestWeightBatchCommand> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new IngestWeightBatchCommand(
            request.BatchId,
            request.DeviceId,
            request.Location,
            request.SentAt,
            request
                .Readings.Select(r => new WeightReadingItem(
                    r.Weight,
                    r.Count,
                    r.FirstTimestamp,
                    r.LastTimestamp,
                    r.StableCount
                ))
                .ToList()
        );

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

        return Ok();
    }
}
