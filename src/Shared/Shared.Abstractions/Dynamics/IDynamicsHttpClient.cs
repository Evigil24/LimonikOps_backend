namespace LimonikOne.Shared.Abstractions.Dynamics;

public interface IDynamicsHttpClient
{
    Task<IReadOnlyList<T>> GetAsync<T>(
        string entitySet,
        string? filter = null,
        string? select = null,
        string? expand = null,
        CancellationToken cancellationToken = default
    );

    Task<T?> GetByKeyAsync<T>(
        string entitySet,
        Guid key,
        CancellationToken cancellationToken = default
    );

    Task<T?> PostAsync<T>(
        string entitySet,
        object payload,
        CancellationToken cancellationToken = default
    );

    Task PatchAsync(
        string entitySet,
        Guid key,
        object payload,
        CancellationToken cancellationToken = default
    );
}
