using LimonikOne.Modules.Person.Domain.VendorClassifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Person.Infrastructure.Database.Configurations.VendorClassifications;

internal sealed class VendorClassificationConfiguration
    : IEntityTypeConfiguration<VendorClassificationEntity>
{
    public void Configure(EntityTypeBuilder<VendorClassificationEntity> builder)
    {
        builder.ToTable("vendor_classifications");

        builder.HasKey(vc => vc.Id);

        builder
            .Property(vc => vc.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => VendorClassificationId.From(value))
            .ValueGeneratedNever();

        builder.Property(vc => vc.DisplayId).HasColumnName("display_id").UseIdentityAlwaysColumn();
        builder.HasIndex(vc => vc.DisplayId).IsUnique();

        builder.Property(vc => vc.Name).HasColumnName("name").HasMaxLength(300).IsRequired();

        builder.Property(vc => vc.Description).HasColumnName("description").HasMaxLength(1000);

        builder
            .Property(vc => vc.ParentId)
            .HasColumnName("parent_id")
            .HasConversion(
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? VendorClassificationId.From(value.Value) : null
            );

        builder
            .HasOne(vc => vc.Parent)
            .WithMany(vc => vc.Children)
            .HasForeignKey(vc => vc.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
