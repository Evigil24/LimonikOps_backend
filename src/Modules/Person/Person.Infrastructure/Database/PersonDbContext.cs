using LimonikOne.Modules.Person.Domain.Vendors;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Person.Infrastructure.Database;

internal sealed class PersonDbContext : DbContext
{
    public DbSet<VendorEntity> Vendors => Set<VendorEntity>();

    public PersonDbContext(DbContextOptions<PersonDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("person");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonDbContext).Assembly);
    }
}
