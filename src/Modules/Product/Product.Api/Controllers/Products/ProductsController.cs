using FluentValidation;
using LimonikOne.Modules.Product.Api.Controllers.Products.Requests;
using LimonikOne.Modules.Product.Application.Products.Create;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Product.Api.Controllers.Products;

[ApiController]
[Tags("Product")]
[Route("api/product/products")]
public sealed class ProductsController : ControllerBase
{
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
