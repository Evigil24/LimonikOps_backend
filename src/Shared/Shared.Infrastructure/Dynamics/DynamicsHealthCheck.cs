using LimonikOne.Shared.Abstractions.Dynamics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace LimonikOne.Shared.Infrastructure.Dynamics;

public sealed class DynamicsHealthCheck : IHealthCheck
{
    private readonly IDynamicsHttpClient _dynamicsClient;
    private readonly DynamicsOptions _options;

    public DynamicsHealthCheck(
        IDynamicsHttpClient dynamicsClient,
        IOptions<DynamicsOptions> options
    )
    {
        _dynamicsClient = dynamicsClient;
        _options = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        if (
            string.IsNullOrWhiteSpace(_options.TenantId)
            || string.IsNullOrWhiteSpace(_options.ClientId)
            || string.IsNullOrWhiteSpace(_options.Resource)
            || string.IsNullOrWhiteSpace(_options.HealthCheckEntitySet)
        )
        {
            return HealthCheckResult.Unhealthy(
                "Dynamics is not configured. Set TenantId, ClientId, Resource, and HealthCheckEntitySet in appsettings."
            );
        }

        try
        {
            // Call a lightweight collection endpoint (returns { "value": [] }) to verify auth and connectivity.
            await _dynamicsClient.GetAsync<object>(_options.HealthCheckEntitySet, null, null, null, cancellationToken);
            return HealthCheckResult.Healthy("Dynamics connection successful.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Dynamics connection failed.", ex);
        }
    }
}
