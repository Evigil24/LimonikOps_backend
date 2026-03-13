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
        )
        {
            return HealthCheckResult.Unhealthy(
                "Dynamics is not configured. Set TenantId, ClientId, and Resource in appsettings."
            );
        }

        try
        {
            // Call a minimal endpoint to verify auth and connectivity.
            // organizations always exists and has one row per Dataverse environment.
            await _dynamicsClient.GetAsync<OrganizationRef>(
                "organizations",
                filter: null,
                select: "organizationid",
                expand: null,
                cancellationToken
            );
            return HealthCheckResult.Healthy("Dynamics connection successful.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Dynamics connection failed.", ex);
        }
    }

    private sealed record OrganizationRef(Guid OrganizationId);
}
