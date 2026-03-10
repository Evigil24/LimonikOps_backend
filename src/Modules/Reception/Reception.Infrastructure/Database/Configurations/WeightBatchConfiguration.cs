using LimonikOne.Modules.Reception.Domain.Weights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Reception.Infrastructure.Database.Configurations;

internal sealed class WeightBatchConfiguration : IEntityTypeConfiguration<WeightBatchEntity>
{
    public void Configure(EntityTypeBuilder<WeightBatchEntity> builder)
    {
        builder.ToTable("weight_batches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => WeightBatchId.From(value))
            .ValueGeneratedNever();

        builder.Property(b => b.DisplayId)
            .HasColumnName("display_id")
            .UseIdentityAlwaysColumn();

        builder.HasIndex(b => b.DisplayId)
            .IsUnique();

        builder.Property(b => b.ExternalBatchId)
            .HasColumnName("external_batch_id")
            .IsRequired();

        builder.HasIndex(b => b.ExternalBatchId)
            .IsUnique();

        builder.Property(b => b.DeviceId)
            .HasColumnName("device_id")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.Location)
            .HasColumnName("location")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(b => b.SentAt)
            .HasColumnName("sent_at")
            .IsRequired();

        builder.Property(b => b.ReceivedAt)
            .HasColumnName("received_at")
            .IsRequired();

        builder.OwnsMany(b => b.Readings, reading =>
        {
            reading.ToTable("weight_readings");

            reading.WithOwner().HasForeignKey("weight_batch_id");

            reading.HasKey(r => r.Id);

            reading.Property(r => r.Id)
                .HasColumnName("id")
                .HasConversion(
                    id => id.Value,
                    value => WeightReadingId.From(value))
                .ValueGeneratedNever();

            reading.Property(r => r.Weight)
                .HasColumnName("weight")
                .HasPrecision(18, 4)
                .IsRequired();

            reading.Property(r => r.Count)
                .HasColumnName("count")
                .IsRequired();

            reading.Property(r => r.FirstTimestamp)
                .HasColumnName("first_timestamp")
                .IsRequired();

            reading.Property(r => r.LastTimestamp)
                .HasColumnName("last_timestamp")
                .IsRequired();

            reading.Property(r => r.StableCount)
                .HasColumnName("stable_count")
                .IsRequired();
        });
    }
}
