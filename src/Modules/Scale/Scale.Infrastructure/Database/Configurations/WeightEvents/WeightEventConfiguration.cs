using LimonikOne.Modules.Scale.Domain.WeightEvents;
using LimonikOne.Modules.Scale.Domain.WeightReadings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Configurations.WeightEvents;

internal sealed class WeightEventConfiguration : IEntityTypeConfiguration<WeightEventEntity>
{
    public void Configure(EntityTypeBuilder<WeightEventEntity> builder)
    {
        builder.ToTable("weight_events");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => WeightEventId.From(value))
            .ValueGeneratedNever();

        builder.Property(e => e.DisplayId).HasColumnName("display_id").UseIdentityAlwaysColumn();

        builder.HasIndex(e => e.DisplayId).IsUnique();

        builder.Property(e => e.DeviceId).HasColumnName("device_id").HasMaxLength(100).IsRequired();

        builder.Property(e => e.Location).HasColumnName("location").HasMaxLength(200).IsRequired();

        builder
            .Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(e => new { e.DeviceId, e.Status });

        builder.Property(e => e.StartedAt).HasColumnName("started_at").IsRequired();

        builder.Property(e => e.EndedAt).HasColumnName("ended_at");

        builder
            .Property(e => e.PeakWeight)
            .HasColumnName("peak_weight")
            .HasPrecision(18, 4)
            .IsRequired();

        builder.OwnsMany(
            e => e.Measurements,
            measurement =>
            {
                measurement.ToTable("weight_measurements");

                measurement.WithOwner().HasForeignKey("weight_event_id");

                measurement.HasKey(m => m.Id);

                measurement
                    .Property(m => m.Id)
                    .HasColumnName("id")
                    .HasConversion(id => id.Value, value => WeightMeasurementId.From(value))
                    .ValueGeneratedNever();

                measurement
                    .Property(m => m.Weight)
                    .HasColumnName("weight")
                    .HasPrecision(18, 4)
                    .IsRequired();

                measurement.Property(m => m.Timestamp).HasColumnName("timestamp").IsRequired();

                measurement.Property(m => m.StableCount).HasColumnName("stable_count").IsRequired();

                measurement
                    .Property(m => m.SourceReadingId)
                    .HasColumnName("source_reading_id")
                    .HasConversion(id => id.Value, value => WeightReadingId.From(value))
                    .IsRequired();
            }
        );
    }
}
