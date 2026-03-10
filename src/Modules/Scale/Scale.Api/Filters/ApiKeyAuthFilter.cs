using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace LimonikOne.Modules.Scale.Api.Filters;

internal sealed class ApiKeyAuthFilter : IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var configuredKey = _configuration["ScalePulse:ApiKey"];

        if (string.IsNullOrEmpty(configuredKey))
        {
            await next();
            return;
        }

        if (
            !context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedKey)
            || string.IsNullOrEmpty(providedKey)
        )
        {
            context.Result = new ObjectResult(
                new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Detail = "API key is missing.",
                }
            )
            {
                StatusCode = StatusCodes.Status401Unauthorized,
            };
            return;
        }

        var configuredBytes = Encoding.UTF8.GetBytes(configuredKey);
        var providedBytes = Encoding.UTF8.GetBytes(providedKey.ToString());

        if (!CryptographicOperations.FixedTimeEquals(configuredBytes, providedBytes))
        {
            context.Result = new ObjectResult(
                new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Detail = "Invalid API key.",
                }
            )
            {
                StatusCode = StatusCodes.Status401Unauthorized,
            };
            return;
        }

        await next();
    }
}
