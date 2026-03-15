using LimonikOne.Modules.Scale.Domain.VehicleScaleRecords;
using LimonikOne.Modules.Scale.Domain.WeightReadings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Configurations.VehicleScaleRecords;

internal sealed class VehicleScaleRecordConfiguration
    : IEntityTypeConfiguration<VehicleScaleRecordEntity>
{
    public void Configure(EntityTypeBuilder<VehicleScaleRecordEntity> builder)
    {
        builder.ToTable("vehicle_scale_records");

        builder.HasKey(record => record.Id);

        builder
            .Property(record => record.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => VehicleScaleRecordId.From(value))
            .ValueGeneratedNever();

        builder
            .Property(record => record.Type)
            .HasColumnName("type_id")
            .HasConversion(type => type.Id, value => VehicleScaleRecordType.FromId(value))
            .IsRequired();

        builder
            .Property(record => record.Status)
            .HasColumnName("status_id")
            .HasConversion(status => status.Id, value => VehicleScaleRecordStatus.FromId(value))
            .IsRequired();

        builder.Property(record => record.StartedAt).HasColumnName("started_at").IsRequired();

        builder.Property(record => record.ClosedAt).HasColumnName("closed_at");

        builder
            .Property(record => record.FirstWeight)
            .HasColumnName("first_weight")
            .HasPrecision(18, 4);

        builder
            .Property(record => record.FirstWeightId)
            .HasColumnName("first_weight_id")
            .HasConversion(
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? WeightReadingId.From(value.Value) : (WeightReadingId?)null
            );

        builder
            .Property(record => record.SecondWeight)
            .HasColumnName("second_weight")
            .HasPrecision(18, 4);

        builder
            .Property(record => record.SecondWeightId)
            .HasColumnName("second_weight_id")
            .HasConversion(
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? WeightReadingId.From(value.Value) : (WeightReadingId?)null
            );

        builder
            .Property(record => record.NetWeight)
            .HasColumnName("net_weight")
            .HasPrecision(18, 4);

        builder.Property(record => record.CreatedAt).HasColumnName("created_at").IsRequired();

        builder
            .Property(record => record.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(100)
            .IsRequired();
    }
}
