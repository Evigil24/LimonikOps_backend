using LimonikOne.Modules.Person.Domain.Customers;
using LimonikOne.Modules.Person.Domain.VendorClassifications;
using LimonikOne.Modules.Person.Domain.Vendors;
using Microsoft.EntityFrameworkCore;

namespace LimonikOne.Modules.Person.Infrastructure.Database;

internal sealed class PersonDbContext(DbContextOptions<PersonDbContext> options)
    : DbContext(options)
{
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    public DbSet<VendorEntity> Vendors => Set<VendorEntity>();
    public DbSet<VendorClassificationEntity> VendorClassifications =>
        Set<VendorClassificationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("person");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonDbContext).Assembly);
    }
}
