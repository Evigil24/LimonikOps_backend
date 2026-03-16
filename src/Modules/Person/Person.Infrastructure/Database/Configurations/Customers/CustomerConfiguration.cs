using LimonikOne.Modules.Person.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Person.Infrastructure.Database.Configurations.Customers;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(customer => customer.Id);

        builder
            .Property(customer => customer.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => CustomerId.From(value))
            .ValueGeneratedNever();

        builder
            .Property(customer => customer.DisplayId)
            .HasColumnName("display_id")
            .UseIdentityAlwaysColumn();
        builder.HasIndex(customer => customer.DisplayId).IsUnique();

        builder
            .Property(customer => customer.AccountNumber)
            .HasColumnName("account_number")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(customer => customer.GroupId)
            .HasColumnName("group_id")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(customer => customer.Name)
            .HasColumnName("name")
            .HasMaxLength(300)
            .IsRequired();

        builder
            .Property(customer => customer.SearchName)
            .HasColumnName("search_name")
            .HasMaxLength(300)
            .IsRequired();

        builder
            .Property(customer => customer.PartyNumber)
            .HasColumnName("party_number")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(customer => customer.RFCFederalTaxNumber)
            .HasColumnName("rfc_federal_tax_number")
            .HasMaxLength(100)
            .IsRequired();
    }
}
