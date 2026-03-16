using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;
using LimonikOne.Modules.Scale.Domain.WeightBatches;
using LimonikOne.Modules.Scale.Domain.WeightEvents;
using LimonikOne.Modules.Scale.Domain.WeightReadings;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Scale.Infrastructure.Database;

internal sealed class ScaleDbContext(DbContextOptions<ScaleDbContext> options) : DbContext(options)
{
    public DbSet<WeightBatchEntity> WeightBatches => Set<WeightBatchEntity>();
    public DbSet<WeightReading> WeightReadings => Set<WeightReading>();
    public DbSet<WeightEventEntity> WeightEvents => Set<WeightEventEntity>();
    public DbSet<VehicleScaleRecordEntity> VehicleScaleRecords => Set<VehicleScaleRecordEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("scale");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScaleDbContext).Assembly);
    }
}
