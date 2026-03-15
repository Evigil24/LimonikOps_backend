using LimonikOne.Modules.Product.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Product.Infrastructure.Database.Configurations.Items;

internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("items");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => ItemId.From(value))
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
            .IsRequired()
            .HasConversion(v => v.Id, id => Variety.FromId(id));
        builder
            .Property(p => p.Handling)
            .HasColumnName("handling")
            .IsRequired()
            .HasConversion(h => h.Id, id => Handling.FromId(id));
        builder
            .Property(p => p.Certification)
            .HasColumnName("certification")
            .IsRequired()
            .HasConversion(c => c.Id, id => Certification.FromId(id));
        builder
            .Property(p => p.Stage)
            .HasColumnName("stage")
            .IsRequired()
            .HasConversion(s => s.Id, id => Stage.FromId(id));

        builder.HasIndex(p => p.ItemNumber).IsUnique();
        builder.HasIndex(p => p.SearchName);
    }
}
