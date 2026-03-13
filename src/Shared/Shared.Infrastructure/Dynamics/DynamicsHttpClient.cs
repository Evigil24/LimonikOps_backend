using System.Net.Http.Json;
using System.Text.Json;
using LimonikOne.Shared.Abstractions.Dynamics;
using Microsoft.Extensions.Options;

namespace LimonikOne.Shared.Infrastructure.Dynamics;

public sealed class DynamicsHttpClient : IDynamicsHttpClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient _httpClient;
    private readonly DynamicsOptions _options;

    public DynamicsHttpClient(HttpClient httpClient, IOptions<DynamicsOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<IReadOnlyList<T>> GetAsync<T>(
        string entitySet,
        string? filter = null,
        string? select = null,
        string? expand = null,
        CancellationToken cancellationToken = default
    )
    {
        var baseUrl = GetBaseUrl();
        var queryParams = BuildQueryString(filter, select, expand);
        var url = $"{baseUrl}{entitySet}{queryParams}";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var wrapper =
            JsonSerializer.Deserialize<ODataValueWrapper<T>>(content, JsonOptions)
            ?? throw new InvalidOperationException(
                $"Failed to deserialize OData response for {entitySet}."
            );

        return wrapper.Value ?? [];
    }

    public async Task<T?> GetByKeyAsync<T>(
        string entitySet,
        Guid key,
        CancellationToken cancellationToken = default
    )
    {
        var baseUrl = GetBaseUrl();
        var url = $"{baseUrl}{entitySet}({key})";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(content, JsonOptions);
    }

    public async Task<T?> PostAsync<T>(
        string entitySet,
        object payload,
        CancellationToken cancellationToken = default
    )
    {
        var baseUrl = GetBaseUrl();
        var url = $"{baseUrl}{entitySet}";

        var response = await _httpClient.PostAsJsonAsync(
            url,
            payload,
            JsonOptions,
            cancellationToken
        );
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return default;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return string.IsNullOrEmpty(content)
            ? default
            : JsonSerializer.Deserialize<T>(content, JsonOptions);
    }

    public async Task PatchAsync(
        string entitySet,
        Guid key,
        object payload,
        CancellationToken cancellationToken = default
    )
    {
        var baseUrl = GetBaseUrl();
        var url = $"{baseUrl}{entitySet}({key})";

        var content = JsonContent.Create(payload, options: JsonOptions);
        var response = await _httpClient.PatchAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private string GetBaseUrl()
    {
        var resource = _options.Resource.TrimEnd('/');
        var version = _options.ApiVersion;
        return $"{resource}/api/data/v{version}/";
    }

    private static string BuildQueryString(string? filter, string? select, string? expand)
    {
        var parts = new List<string>();

        if (!string.IsNullOrEmpty(filter))
        {
            parts.Add($"$filter={Uri.EscapeDataString(filter)}");
        }

        if (!string.IsNullOrEmpty(select))
        {
            parts.Add($"$select={Uri.EscapeDataString(select)}");
        }

        if (!string.IsNullOrEmpty(expand))
        {
            parts.Add($"$expand={Uri.EscapeDataString(expand)}");
        }

        return parts.Count == 0 ? string.Empty : "?" + string.Join("&", parts);
    }

    private sealed class ODataValueWrapper<T>
    {
        public List<T>? Value { get; set; }
    }
}
