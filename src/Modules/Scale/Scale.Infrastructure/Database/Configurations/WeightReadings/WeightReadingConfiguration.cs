using LimonikOne.Modules.Scale.Domain.WeightBatches;
using LimonikOne.Modules.Scale.Domain.WeightReadings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Configurations.WeightReadings;

internal sealed class WeightReadingConfiguration : IEntityTypeConfiguration<WeightReading>
{
    public void Configure(EntityTypeBuilder<WeightReading> builder)
    {
        builder.ToTable("weight_readings");

        builder.HasKey(r => r.Id);

        builder
            .Property(r => r.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => WeightReadingId.From(value))
            .ValueGeneratedNever();

        builder.Property(r => r.DisplayId).HasColumnName("display_id").UseIdentityAlwaysColumn();

        builder.HasIndex(r => r.DisplayId).IsUnique();

        builder
            .Property(r => r.BatchId)
            .HasColumnName("weight_batch_id")
            .HasConversion(id => id.Value, value => WeightBatchId.From(value))
            .IsRequired();

        builder.HasIndex(r => r.BatchId);

        builder.Property(r => r.Weight).HasColumnName("weight").HasPrecision(18, 4).IsRequired();

        builder.Property(r => r.Count).HasColumnName("count").IsRequired();

        builder.Property(r => r.FirstTimestamp).HasColumnName("first_timestamp").IsRequired();

        builder.Property(r => r.LastTimestamp).HasColumnName("last_timestamp").IsRequired();

        builder.Property(r => r.StableCount).HasColumnName("stable_count").IsRequired();

        builder
            .HasOne<WeightBatchEntity>()
            .WithMany()
            .HasForeignKey(r => r.BatchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
