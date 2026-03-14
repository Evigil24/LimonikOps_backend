using LimonikOne.Modules.Product.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAggregate = LimonikOne.Modules.Product.Domain.Products.Product;

namespace LimonikOne.Modules.Product.Infrastructure.Database.Configurations.Products;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<ProductAggregate>
{
    public void Configure(EntityTypeBuilder<ProductAggregate> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => ProductId.From(value))
            .ValueGeneratedNever();

        builder.Property(p => p.DisplayId).HasColumnName("display_id").UseIdentityAlwaysColumn();
        builder.HasIndex(p => p.DisplayId).IsUnique();

        builder
            .Property(p => p.ItemNumber)
            .HasColumnName("item_number")
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(p => p.PrimaryName)
            .HasColumnName("primary_name")
            .HasMaxLength(300)
            .IsRequired();
        builder
            .Property(p => p.SearchName)
            .HasColumnName("search_name")
            .HasMaxLength(500)
            .IsRequired();
        builder
            .Property(p => p.Variety)
            .HasColumnName("variety")
            .HasMaxLength(150)
            .IsRequired()
            .HasConversion(v => v.Name, name => Variety.FromName(name));
        builder
            .Property(p => p.Handling)
            .HasColumnName("handling")
            .HasMaxLength(150)
            .IsRequired()
            .HasConversion(h => h.Name, name => Handling.FromName(name));
        builder
            .Property(p => p.Certification)
            .HasColumnName("certification")
            .HasMaxLength(150)
            .IsRequired()
            .HasConversion(c => c.Name, name => Certification.FromName(name));
        builder
            .Property(p => p.Stage)
            .HasColumnName("stage")
            .HasMaxLength(100)
            .IsRequired()
            .HasConversion(s => s.Name, name => Stage.FromName(name));

        builder.HasIndex(p => p.ItemNumber);
        builder.HasIndex(p => p.SearchName);
    }
}
