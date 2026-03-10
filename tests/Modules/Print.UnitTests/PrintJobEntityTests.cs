using FluentAssertions;
using LimonikOne.Modules.Print.Domain.PrintJobs;

namespace LimonikOne.Modules.Print.UnitTests;

public class PrintJobEntityTests
{
    private static PrintJobEntity CreateTestJob() =>
        PrintJobEntity.Create(
            "LABEL_PRINTER_01",
            "^XA^FO20,20^A0N,25,25^FDHello World^FS^XZ",
            "UTF-8",
            "test-label.zpl",
            0,
            new Dictionary<string, string> { ["orderId"] = "12345" }
        );

    [Fact]
    public void Create_Should_Set_Status_To_Queued()
    {
        var job = CreateTestJob();

        job.Status.Should().Be(PrintJobStatus.Queued);
        job.Id.Value.Should().NotBeEmpty();
        job.LogicalPrinterName.Should().Be("LABEL_PRINTER_01");
        job.AttemptNumber.Should().Be(0);
        job.Retryable.Should().BeFalse();
    }

    [Fact]
    public void Claim_Should_Set_Status_To_Claimed()
    {
        var job = CreateTestJob();

        var result = job.Claim("AGENT-01");

        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(PrintJobStatus.Claimed);
        job.ClaimedByAgentId.Should().Be("AGENT-01");
        job.ClaimedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void Claim_Should_Fail_When_Not_Queued()
    {
        var job = CreateTestJob();
        job.Claim("AGENT-01");

        var result = job.Claim("AGENT-02");

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("PrintJob.NotQueued");
    }

    [Fact]
    public void Complete_Should_Set_Status_To_Completed()
    {
        var job = CreateTestJob();
        job.Claim("AGENT-01");
        var completedAt = DateTime.UtcNow;

        var result = job.Complete("AGENT-01", completedAt, "ZDesigner ZT230");

        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(PrintJobStatus.Completed);
        job.CompletedAtUtc.Should().Be(completedAt);
        job.WindowsPrinterName.Should().Be("ZDesigner ZT230");
    }

    [Fact]
    public void Complete_Should_Be_Idempotent()
    {
        var job = CreateTestJob();
        job.Claim("AGENT-01");
        job.Complete("AGENT-01", DateTime.UtcNow, "ZDesigner ZT230");

        var result = job.Complete("AGENT-01", DateTime.UtcNow, "ZDesigner ZT230");

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Complete_Should_Fail_When_Not_Claimed()
    {
        var job = CreateTestJob();

        var result = job.Complete("AGENT-01", DateTime.UtcNow, null);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("PrintJob.NotClaimed");
    }

    [Fact]
    public void Fail_Should_Set_Status_To_Failed()
    {
        var job = CreateTestJob();
        job.Claim("AGENT-01");
        var failedAt = DateTime.UtcNow;

        var result = job.Fail(
            "AGENT-01",
            failedAt,
            "PRINTER_OFFLINE",
            "Printer not found",
            null,
            false,
            1
        );

        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(PrintJobStatus.Failed);
        job.FailedAtUtc.Should().Be(failedAt);
        job.ErrorCode.Should().Be("PRINTER_OFFLINE");
        job.ErrorMessage.Should().Be("Printer not found");
        job.Retryable.Should().BeFalse();
        job.AttemptNumber.Should().Be(1);
    }

    [Fact]
    public void Fail_Should_Be_Idempotent()
    {
        var job = CreateTestJob();
        job.Claim("AGENT-01");
        job.Fail("AGENT-01", DateTime.UtcNow, "ERR", "msg", null, false, 1);

        var result = job.Fail("AGENT-01", DateTime.UtcNow, "ERR", "msg", null, false, 1);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Fail_Should_Fail_When_Not_Claimed()
    {
        var job = CreateTestJob();

        var result = job.Fail("AGENT-01", DateTime.UtcNow, "ERR", "msg", null, false, 1);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("PrintJob.NotClaimed");
    }

    [Fact]
    public void RequeueForRetry_Should_Reset_To_Queued_When_Retryable()
    {
        var job = CreateTestJob();
        job.Claim("AGENT-01");
        job.Fail("AGENT-01", DateTime.UtcNow, "ERR", "msg", null, true, 1);

        var result = job.RequeueForRetry();

        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(PrintJobStatus.Queued);
        job.AttemptNumber.Should().Be(2);
        job.ClaimedByAgentId.Should().BeNull();
        job.ClaimedAtUtc.Should().BeNull();
        job.FailedAtUtc.Should().BeNull();
        job.ErrorCode.Should().BeNull();
        job.ErrorMessage.Should().BeNull();
        job.Retryable.Should().BeFalse();
    }

    [Fact]
    public void RequeueForRetry_Should_Fail_When_Not_Retryable()
    {
        var job = CreateTestJob();
        job.Claim("AGENT-01");
        job.Fail("AGENT-01", DateTime.UtcNow, "ERR", "msg", null, false, 1);

        var result = job.RequeueForRetry();

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("PrintJob.NotRetryable");
    }

    [Fact]
    public void RequeueForRetry_Should_Fail_When_Not_Failed()
    {
        var job = CreateTestJob();

        var result = job.RequeueForRetry();

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Full_Retry_Cycle_Should_Work()
    {
        var job = CreateTestJob();

        // First attempt
        job.Claim("AGENT-01");
        job.Fail("AGENT-01", DateTime.UtcNow, "ERR", "msg", null, true, 1);
        job.RequeueForRetry();

        // Second attempt
        job.Status.Should().Be(PrintJobStatus.Queued);
        job.AttemptNumber.Should().Be(2);

        job.Claim("AGENT-02");
        job.Status.Should().Be(PrintJobStatus.Claimed);
        job.ClaimedByAgentId.Should().Be("AGENT-02");

        job.Complete("AGENT-02", DateTime.UtcNow, "ZDesigner ZT230");
        job.Status.Should().Be(PrintJobStatus.Completed);
    }
}
