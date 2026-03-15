using FluentValidation;
using LimonikOne.Modules.Product.Api.Controllers.Items.Requests;
using LimonikOne.Modules.Product.Application.Items;
using LimonikOne.Modules.Product.Application.Items.Create;
using LimonikOne.Modules.Product.Application.Items.GetAll;
using LimonikOne.Modules.Product.Application.Items.GetById;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Product.Api.Controllers.Items;

[ApiController]
[Tags("Item")]
[Route("api/Item/Items")]
public sealed class ItemsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetAllItemsQuery, IReadOnlyList<ItemDto>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetAllItemsQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetItemByIdQuery, ItemDto> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetItemByIdQuery(id), cancellationToken);

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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateItemRequest request,
        [FromServices] IValidator<CreateItemCommand> validator,
        [FromServices] ICommandHandler<CreateItemCommand, Guid> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateItemCommand(
            request.ItemNumber,
            request.PrimaryName,
            request.SearchName,
            request.VarietyId,
            request.HandlingId,
            request.CertificationId,
            request.StageId
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
                statusCode: StatusCodes.Status409Conflict,
                title: result.Error.Code
            );
        }

        return StatusCode(StatusCodes.Status201Created, new { id = result.Value });
    }
}
