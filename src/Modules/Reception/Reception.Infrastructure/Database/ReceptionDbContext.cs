using LimonikOne.Modules.Reception.Domain.Receptions;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Reception.Infrastructure.Database;

internal sealed class ReceptionDbContext : DbContext
{
    public DbSet<ReceptionEntity> Receptions => Set<ReceptionEntity>();

    public ReceptionDbContext(DbContextOptions<ReceptionDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("reception");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReceptionDbContext).Assembly);
    }
}
