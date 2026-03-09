using FluentValidation;
using LimonikOne.Modules.Reception.Application.Receptions.Create;
using LimonikOne.Modules.Reception.Application.Receptions.Get;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Reception.Api.Controllers;

[ApiController]
[Route("api/receptions")]
public sealed class ReceptionsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateReceptionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateReceptionRequest request,
        [FromServices] IValidator<CreateReceptionCommand> validator,
        [FromServices] ICommandHandler<CreateReceptionCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateReceptionCommand(request.FirstName, request.LastName, request.Notes);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray())));
        }

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error!.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: result.Error.Code);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value },
            new CreateReceptionResponse(result.Value));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReceptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetReceptionByIdQuery, ReceptionDto> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetReceptionByIdQuery(id);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error!.Message,
                statusCode: StatusCodes.Status404NotFound,
                title: result.Error.Code);
        }

        return Ok(result.Value);
    }
}
