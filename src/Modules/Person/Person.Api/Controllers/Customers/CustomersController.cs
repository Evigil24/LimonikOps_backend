using LimonikOne.Modules.Person.Application.Customers;
using LimonikOne.Modules.Person.Application.Customers.GetAll;
using LimonikOne.Modules.Person.Application.Customers.GetById;
using LimonikOne.Modules.Person.Application.Customers.Refresh;
using LimonikOne.Shared.Abstractions.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LimonikOne.Modules.Person.Api.Controllers.Customers;

[ApiController]
[Tags("Customer")]
[Route("api/customers")]
public sealed class CustomersController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetAllCustomersQuery, IReadOnlyList<CustomerDto>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetAllCustomersQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetCustomerByIdQuery, CustomerDto> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetCustomerByIdQuery(id), cancellationToken);

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

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh(
        [FromServices] ICommandHandler<RefreshCustomersCommand> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new RefreshCustomersCommand(), cancellationToken);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error!.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: result.Error.Code
            );
        }

        return Ok();
    }
}
