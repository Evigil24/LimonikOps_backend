using LimonikOne.Modules.Scale.Domain.WeighingEvents;
using LimonikOne.Modules.Scale.Domain.Weights;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Scale.Infrastructure.Database;

internal sealed class ScaleDbContext : DbContext
{
    public DbSet<WeightBatchEntity> WeightBatches => Set<WeightBatchEntity>();
    public DbSet<WeighingEventEntity> WeighingEvents => Set<WeighingEventEntity>();

    public ScaleDbContext(DbContextOptions<ScaleDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("scale");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScaleDbContext).Assembly);
    }
}
