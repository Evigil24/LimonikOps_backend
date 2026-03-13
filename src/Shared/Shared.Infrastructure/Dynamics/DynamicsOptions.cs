namespace LimonikOne.Shared.Infrastructure.Dynamics;

public sealed class DynamicsOptions
{
    public const string SectionName = "Dynamics";

    public string TenantId { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string Resource { get; init; } = string.Empty;
    public string ApiVersion { get; init; } = string.Empty;
}
