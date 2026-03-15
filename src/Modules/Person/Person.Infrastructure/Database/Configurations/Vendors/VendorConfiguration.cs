using LimonikOne.Modules.Person.Domain.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Person.Infrastructure.Database.Configurations.Vendors;

internal sealed class VendorConfiguration : IEntityTypeConfiguration<VendorEntity>
{
    public void Configure(EntityTypeBuilder<VendorEntity> builder)
    {
        builder.ToTable("vendors");

        builder.HasKey(vendor => vendor.Id);

        builder
            .Property(vendor => vendor.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => VendorId.From(value))
            .ValueGeneratedNever();

        builder
            .Property(vendor => vendor.AccountNumber)
            .HasColumnName("account_number")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(vendor => vendor.GroupId)
            .HasColumnName("group_id")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(vendor => vendor.ClassificationId)
            .HasColumnName("classification_id")
            .IsRequired();

        builder
            .Property(vendor => vendor.Name)
            .HasColumnName("name")
            .HasMaxLength(300)
            .IsRequired();

        builder
            .Property(vendor => vendor.SearchName)
            .HasColumnName("search_name")
            .HasMaxLength(300)
            .IsRequired();

        builder
            .Property(vendor => vendor.PartyNumber)
            .HasColumnName("party_number")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(vendor => vendor.RFCFederalTaxNumber)
            .HasColumnName("rfc_federal_tax_number")
            .HasMaxLength(100)
            .IsRequired();
    }
}
