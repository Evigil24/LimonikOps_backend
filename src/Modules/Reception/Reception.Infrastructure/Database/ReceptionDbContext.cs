using LimonikOne.Modules.Reception.Domain.Weights;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Reception.Infrastructure.Database;

internal sealed class ReceptionDbContext : DbContext
{
    public DbSet<WeightBatchEntity> WeightBatches => Set<WeightBatchEntity>();

    public ReceptionDbContext(DbContextOptions<ReceptionDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("reception");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReceptionDbContext).Assembly);
    }
}
