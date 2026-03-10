using FluentValidation;
using LimonikOne.Modules.Print.Api.Controllers.Requests;
using LimonikOne.Modules.Print.Api.Filters;
using LimonikOne.Modules.Print.Application.PrintJobs.Claim;
using LimonikOne.Modules.Print.Application.PrintJobs.Complete;
using LimonikOne.Modules.Print.Application.PrintJobs.Enqueue;
using LimonikOne.Modules.Print.Application.PrintJobs.Fail;
using LimonikOne.Modules.Print.Domain.PrintJobs;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Print.Api.Controllers;

[ApiController]
[Route("api/print-jobs")]
public sealed class PrintJobsController : ControllerBase
{
    [HttpPost]
    [ServiceFilter(typeof(PrintApiKeyAuthFilter))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Enqueue(
        [FromBody] EnqueuePrintJobRequest request,
        [FromServices] IValidator<EnqueuePrintJobCommand> validator,
        [FromServices] ICommandHandler<EnqueuePrintJobCommand, Guid> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new EnqueuePrintJobCommand(
            request.LogicalPrinterName,
            request.ZplPayload,
            request.Encoding,
            request.DocumentName,
            request.Priority,
            request.Metadata
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

        return StatusCode(StatusCodes.Status201Created, new { jobId = result.Value });
    }

    [HttpPost("claim")]
    [ServiceFilter(typeof(PrintApiKeyAuthFilter))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Claim(
        [FromBody] ClaimPrintJobRequest request,
        [FromServices] IValidator<ClaimPrintJobCommand> validator,
        [FromServices] ICommandHandler<ClaimPrintJobCommand, ClaimPrintJobResult?> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new ClaimPrintJobCommand(request.AgentId);

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

        if (result.Value is null)
            return NoContent();

        return Ok(
            new
            {
                success = true,
                job = new
                {
                    jobId = result.Value.JobId,
                    logicalPrinterName = result.Value.LogicalPrinterName,
                    zplPayload = result.Value.ZplPayload,
                    encoding = result.Value.Encoding,
                    documentName = result.Value.DocumentName,
                    queuedAtUtc = result.Value.QueuedAtUtc,
                    priority = result.Value.Priority,
                    metadata = result.Value.Metadata ?? new Dictionary<string, string>(),
                },
                message = (string?)null,
            }
        );
    }

    [HttpPost("{id:guid}/complete")]
    [ServiceFilter(typeof(PrintApiKeyAuthFilter))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(
        [FromRoute] Guid id,
        [FromBody] CompletePrintJobRequest request,
        [FromServices] IValidator<CompletePrintJobCommand> validator,
        [FromServices] ICommandHandler<CompletePrintJobCommand> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new CompletePrintJobCommand(
            PrintJobId.From(id),
            request.AgentId,
            request.CompletedAtUtc,
            request.WindowsPrinterName
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
            var statusCode =
                result.Error!.Code == "PrintJob.NotFound"
                    ? StatusCodes.Status404NotFound
                    : StatusCodes.Status409Conflict;

            return Problem(
                detail: result.Error.Message,
                statusCode: statusCode,
                title: result.Error.Code
            );
        }

        return Ok();
    }

    [HttpPost("{id:guid}/fail")]
    [ServiceFilter(typeof(PrintApiKeyAuthFilter))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Fail(
        [FromRoute] Guid id,
        [FromBody] FailPrintJobRequest request,
        [FromServices] IValidator<FailPrintJobCommand> validator,
        [FromServices] ICommandHandler<FailPrintJobCommand> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new FailPrintJobCommand(
            PrintJobId.From(id),
            request.AgentId,
            request.FailedAtUtc,
            request.ErrorCode,
            request.ErrorMessage,
            request.StackTrace,
            request.Retryable,
            request.AttemptNumber
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
            var statusCode =
                result.Error!.Code == "PrintJob.NotFound"
                    ? StatusCodes.Status404NotFound
                    : StatusCodes.Status409Conflict;

            return Problem(
                detail: result.Error.Message,
                statusCode: statusCode,
                title: result.Error.Code
            );
        }

        return Ok();
    }
}
