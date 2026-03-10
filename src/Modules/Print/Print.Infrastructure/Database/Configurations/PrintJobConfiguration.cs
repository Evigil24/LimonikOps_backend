using System.Text.Json;
using LimonikOne.Modules.Print.Domain.PrintJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LimonikOne.Modules.Print.Infrastructure.Database.Configurations;

internal sealed class PrintJobConfiguration : IEntityTypeConfiguration<PrintJobEntity>
{
    public void Configure(EntityTypeBuilder<PrintJobEntity> builder)
    {
        builder.ToTable("print_jobs");

        builder.HasKey(j => j.Id);

        builder
            .Property(j => j.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, value => PrintJobId.From(value))
            .ValueGeneratedNever();

        builder.Property(j => j.DisplayId).HasColumnName("display_id").UseIdentityAlwaysColumn();
        builder.HasIndex(j => j.DisplayId).IsUnique();

        builder
            .Property(j => j.LogicalPrinterName)
            .HasColumnName("logical_printer_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(j => j.ZplPayload).HasColumnName("zpl_payload").IsRequired();

        builder.Property(j => j.Encoding).HasColumnName("encoding").HasMaxLength(50);

        builder.Property(j => j.DocumentName).HasColumnName("document_name").HasMaxLength(500);

        builder.Property(j => j.QueuedAtUtc).HasColumnName("queued_at_utc").IsRequired();

        builder.Property(j => j.Priority).HasColumnName("priority").IsRequired();

        builder
            .Property(j => j.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("jsonb")
            .HasConversion(
                v => v != null ? JsonSerializer.Serialize(v, (JsonSerializerOptions?)null) : null,
                v =>
                    v != null
                        ? JsonSerializer.Deserialize<Dictionary<string, string>>(
                            v,
                            (JsonSerializerOptions?)null
                        )
                        : null,
                new ValueComparer<Dictionary<string, string>?>(
                    (a, b) =>
                        a == null && b == null
                        || a != null
                            && b != null
                            && JsonSerializer.Serialize(a, (JsonSerializerOptions?)null)
                                == JsonSerializer.Serialize(b, (JsonSerializerOptions?)null),
                    v =>
                        v == null
                            ? 0
                            : JsonSerializer
                                .Serialize(v, (JsonSerializerOptions?)null)
                                .GetHashCode(),
                    v => v == null ? null : new Dictionary<string, string>(v)
                )
            );

        builder
            .Property(j => j.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(j => j.ClaimedByAgentId)
            .HasColumnName("claimed_by_agent_id")
            .HasMaxLength(100);

        builder.Property(j => j.ClaimedAtUtc).HasColumnName("claimed_at_utc");

        builder.Property(j => j.CompletedAtUtc).HasColumnName("completed_at_utc");

        builder
            .Property(j => j.WindowsPrinterName)
            .HasColumnName("windows_printer_name")
            .HasMaxLength(200);

        builder.Property(j => j.FailedAtUtc).HasColumnName("failed_at_utc");

        builder.Property(j => j.ErrorCode).HasColumnName("error_code").HasMaxLength(100);

        builder.Property(j => j.ErrorMessage).HasColumnName("error_message").HasMaxLength(2000);

        builder.Property(j => j.StackTrace).HasColumnName("stack_trace");

        builder.Property(j => j.Retryable).HasColumnName("retryable").IsRequired();

        builder.Property(j => j.AttemptNumber).HasColumnName("attempt_number").IsRequired();

        builder.HasIndex(j => new
        {
            j.Status,
            j.Priority,
            j.QueuedAtUtc,
        });
    }
}
