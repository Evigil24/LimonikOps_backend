using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LimonikOne.Shared.Infrastructure.Dynamics;

public sealed class DynamicsAuthHandler(
    IHttpClientFactory httpClientFactory,
    ILogger<DynamicsAuthHandler> logger,
    IOptions<DynamicsOptions> options
) : DelegatingHandler
{
    private const int RefreshBeforeExpirySeconds = 300; // 5 minutes

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<DynamicsAuthHandler> _logger = logger;
    private readonly DynamicsOptions _options = options.Value;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private string? _cachedToken;
    private DateTimeOffset _tokenExpiresAt;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var token = await GetTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (
                _cachedToken is not null
                && DateTimeOffset.UtcNow < _tokenExpiresAt.AddSeconds(-RefreshBeforeExpirySeconds)
            )
            {
                return _cachedToken;
            }

            var token = await AcquireTokenAsync(cancellationToken);
            _cachedToken = token.AccessToken;
            _tokenExpiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
            return _cachedToken;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<TokenResponse> AcquireTokenAsync(CancellationToken cancellationToken)
    {
        var tokenClient = _httpClientFactory.CreateClient("Dynamics.Token");

        var tokenUrl = $"https://login.microsoftonline.com/{_options.TenantId}/oauth2/v2.0/token";
        var scope = $"{_options.Resource.TrimEnd('/')}/.default";

        var formContent = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["scope"] = scope,
            }
        );

        var response = await tokenClient.PostAsync(tokenUrl, formContent, cancellationToken);
        await response.EnsureSuccessOrThrowAsync(
            "Dynamics token acquisition",
            tokenUrl,
            _logger,
            cancellationToken
        );

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using (var doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            var accessToken =
                root.GetProperty("access_token").GetString()
                ?? throw new InvalidOperationException("Token response missing access_token.");
            var expiresIn = root.GetProperty("expires_in").GetInt32();
            return new TokenResponse(accessToken, expiresIn);
        }
    }

    private sealed record TokenResponse(string AccessToken, int ExpiresIn);
}
