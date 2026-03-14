using System.Net;
using Microsoft.Extensions.Logging;

namespace LimonikOne.Shared.Infrastructure.Dynamics;

internal static class HttpResponseMessageExtensions
{
    private const int MaxBodyLengthForLog = 2000;
    private const int MaxBodyLengthForException = 500;

    public static async Task EnsureSuccessOrThrowAsync(
        this HttpResponseMessage response,
        string operation,
        string? requestUrl,
        ILogger logger,
        CancellationToken cancellationToken = default
    )
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var url = requestUrl ?? response.RequestMessage?.RequestUri?.ToString() ?? "(unknown)";
        var statusCode = (int)response.StatusCode;

        string body;
        try
        {
            body = await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            body = $"(failed to read response body: {ex.Message})";
        }

        var bodyForLog = Truncate(body, MaxBodyLengthForLog);
        var bodyForException = Truncate(body, MaxBodyLengthForException);

        logger.LogError(
            "Dynamics HTTP request failed. Operation: {Operation}, URL: {Url}, StatusCode: {StatusCode}, ResponseBody: {ResponseBody}",
            operation,
            url,
            statusCode,
            bodyForLog
        );

        var message =
            $"Dynamics request failed ({operation}). Status: {statusCode} {response.StatusCode}. Url: {url}. Response: {bodyForException}";
        throw new HttpRequestException(message, null, (HttpStatusCode?)response.StatusCode);
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "(empty)";
        }

        return value.Length <= maxLength ? value : value[..maxLength] + "...";
    }
}
