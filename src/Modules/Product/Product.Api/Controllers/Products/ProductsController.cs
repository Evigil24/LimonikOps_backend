using FluentValidation;
using LimonikOne.Modules.Product.Api.Controllers.Products.Requests;
using LimonikOne.Modules.Product.Application.Products;
using LimonikOne.Modules.Product.Application.Products.Create;
using LimonikOne.Modules.Product.Application.Products.GetAll;
using LimonikOne.Modules.Product.Application.Products.GetById;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Product.Api.Controllers.Products;

[ApiController]
[Tags("Product")]
[Route("api/product/products")]
public sealed class ProductsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetAllProductsQuery, IReadOnlyList<ProductDto>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetAllProductsQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetProductByIdQuery, ProductDto> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetProductByIdQuery(id), cancellationToken);

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
        [FromBody] CreateProductRequest request,
        [FromServices] IValidator<CreateProductCommand> validator,
        [FromServices] ICommandHandler<CreateProductCommand, Guid> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateProductCommand(
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
