namespace LimonikOne.Shared.Infrastructure.Database;

/// <summary>
/// Generates UUIDv7 values (time-ordered UUIDs).
/// </summary>
public static class UuidV7Generator
{
    public static Guid Generate()
    {
        return Guid.CreateVersion7();
    }
}
