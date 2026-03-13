using LimonikOne.Modules.Scale.Domain.WeightBatches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Configurations.WeightBatches;

internal sealed class WeightBatchConfiguration : IEntityTypeConfiguration<WeightBatchEntity>
{
    public void Configure(EntityTypeBuilder<WeightBatchEntity> builder)
    {
        builder.ToTable("weight_batches");

        builder.HasKey(b => b.Id);

        builder
            .Property(b => b.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => WeightBatchId.From(value))
            .ValueGeneratedNever();

        builder.Property(b => b.DisplayId).HasColumnName("display_id").UseIdentityAlwaysColumn();

        builder.HasIndex(b => b.DisplayId).IsUnique();

        builder.Property(b => b.ExternalBatchId).HasColumnName("external_batch_id").IsRequired();

        builder.HasIndex(b => b.ExternalBatchId).IsUnique();

        builder.Property(b => b.DeviceId).HasColumnName("device_id").HasMaxLength(100).IsRequired();

        builder.Property(b => b.Location).HasColumnName("location").HasMaxLength(200).IsRequired();

        builder.Property(b => b.SentAt).HasColumnName("sent_at").IsRequired();

        builder.Property(b => b.ReceivedAt).HasColumnName("received_at").IsRequired();
    }
}
