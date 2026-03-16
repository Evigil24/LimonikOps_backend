using LimonikOne.Modules.Print.Domain.PrintJobs;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Print.Infrastructure.Database;

internal sealed class PrintDbContext(DbContextOptions<PrintDbContext> options) : DbContext(options)
{
    public DbSet<PrintJobEntity> PrintJobs => Set<PrintJobEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("print");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PrintDbContext).Assembly);
    }
}
