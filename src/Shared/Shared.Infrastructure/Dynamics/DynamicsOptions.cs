namespace LimonikOne.Shared.Infrastructure.Dynamics;

public sealed class DynamicsOptions
{
    public const string SectionName = "Dynamics";

    public string TenantId { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string Resource { get; init; } = string.Empty;

    /// <summary>
    /// Entity set used by the health check (e.g. LeaseExecutoryCostsAccountsCollection).
    /// A lightweight collection that returns { "value": [] } is required.
    /// </summary>
    public string HealthCheckEntitySet { get; init; } = string.Empty;
}
