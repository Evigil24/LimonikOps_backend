using LimonikOne.Modules.Print.Domain.PrintJobs;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Print.Infrastructure.Database;

internal sealed class PrintDbContext : DbContext
{
    public DbSet<PrintJobEntity> PrintJobs => Set<PrintJobEntity>();

    public PrintDbContext(DbContextOptions<PrintDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("print");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PrintDbContext).Assembly);
    }
}
