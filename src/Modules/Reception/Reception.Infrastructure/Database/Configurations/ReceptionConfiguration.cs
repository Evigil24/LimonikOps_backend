using LimonikOne.Modules.Reception.Domain.Receptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Reception.Infrastructure.Database.Configurations;

internal sealed class ReceptionConfiguration : IEntityTypeConfiguration<ReceptionEntity>
{
    public void Configure(EntityTypeBuilder<ReceptionEntity> builder)
    {
        builder.ToTable("receptions");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => ReceptionId.From(value))
            .ValueGeneratedNever();

        builder.Property(r => r.DisplayId)
            .HasColumnName("display_id")
            .UseIdentityAlwaysColumn();

        builder.HasIndex(r => r.DisplayId)
            .IsUnique();

        builder.OwnsOne(r => r.GuestName, guestName =>
        {
            guestName.Property(g => g.FirstName)
                .HasColumnName("guest_first_name")
                .HasMaxLength(100)
                .IsRequired();

            guestName.Property(g => g.LastName)
                .HasColumnName("guest_last_name")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(r => r.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(r => r.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500);

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(r => r.CheckedInAt)
            .HasColumnName("checked_in_at");
    }
}
