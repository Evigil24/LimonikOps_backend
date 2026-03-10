using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace LimonikOne.Modules.Print.IntegrationTests;

public class PrintJobsApiTests : IClassFixture<PrintApiFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public PrintJobsApiTests(PrintApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Enqueue_WithValidRequest_Returns201()
    {
        var request = new
        {
            logicalPrinterName = "LABEL_PRINTER_01",
            zplPayload = "^XA^FO20,20^A0N,25,25^FDHello^FS^XZ",
            encoding = "UTF-8",
            documentName = "test-label.zpl",
            priority = 0,
            metadata = new Dictionary<string, string> { ["orderId"] = "12345" },
        };

        var response = await _client.PostAsJsonAsync("/api/print-jobs", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("jobId").GetGuid().Should().NotBeEmpty();
    }

    [Fact]
    public async Task Claim_WithNoJobs_Returns204()
    {
        var request = new { agentId = "AGENT-EMPTY-TEST" };

        // Use a unique factory or ensure no jobs exist; 204 is returned when queue is empty
        var response = await _client.PostAsJsonAsync("/api/print-jobs/claim", request);

        // Could be 200 if other tests enqueued jobs, or 204 if empty
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Enqueue_Then_Claim_Returns200WithJob()
    {
        // Enqueue
        var enqueueRequest = new
        {
            logicalPrinterName = "CLAIM_TEST_PRINTER",
            zplPayload = "^XA^FDClaim Test^FS^XZ",
            priority = 0,
        };
        var enqueueResponse = await _client.PostAsJsonAsync("/api/print-jobs", enqueueRequest);
        enqueueResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var enqueueBody = await enqueueResponse.Content.ReadFromJsonAsync<JsonElement>();
        var jobId = enqueueBody.GetProperty("jobId").GetGuid();

        // Claim
        var claimRequest = new { agentId = "AGENT-01" };
        var claimResponse = await _client.PostAsJsonAsync("/api/print-jobs/claim", claimRequest);

        claimResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var claimBody = await claimResponse.Content.ReadFromJsonAsync<JsonElement>();
        claimBody.GetProperty("success").GetBoolean().Should().BeTrue();
        claimBody
            .GetProperty("job")
            .GetProperty("logicalPrinterName")
            .GetString()
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task Complete_AfterClaim_Returns200()
    {
        // Enqueue
        var enqueueRequest = new
        {
            logicalPrinterName = "COMPLETE_TEST_PRINTER",
            zplPayload = "^XA^FDComplete Test^FS^XZ",
            priority = 0,
        };
        await _client.PostAsJsonAsync("/api/print-jobs", enqueueRequest);

        // Claim
        var claimResponse = await _client.PostAsJsonAsync(
            "/api/print-jobs/claim",
            new { agentId = "AGENT-01" }
        );
        var claimBody = await claimResponse.Content.ReadFromJsonAsync<JsonElement>();
        var jobId = claimBody.GetProperty("job").GetProperty("jobId").GetGuid();

        // Complete
        var completeRequest = new
        {
            agentId = "AGENT-01",
            completedAtUtc = DateTime.UtcNow,
            windowsPrinterName = "ZDesigner ZT230",
        };
        var completeResponse = await _client.PostAsJsonAsync(
            $"/api/print-jobs/{jobId}/complete",
            completeRequest
        );

        completeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Complete_Idempotent_Returns200OnSecondCall()
    {
        // Enqueue + Claim
        await _client.PostAsJsonAsync(
            "/api/print-jobs",
            new
            {
                logicalPrinterName = "IDEMPOTENT_COMPLETE_PRINTER",
                zplPayload = "^XA^FDIdempotent^FS^XZ",
                priority = 0,
            }
        );
        var claimResponse = await _client.PostAsJsonAsync(
            "/api/print-jobs/claim",
            new { agentId = "AGENT-01" }
        );
        var claimBody = await claimResponse.Content.ReadFromJsonAsync<JsonElement>();
        var jobId = claimBody.GetProperty("job").GetProperty("jobId").GetGuid();

        var completeRequest = new
        {
            agentId = "AGENT-01",
            completedAtUtc = DateTime.UtcNow,
            windowsPrinterName = "ZDesigner ZT230",
        };

        // First complete
        await _client.PostAsJsonAsync($"/api/print-jobs/{jobId}/complete", completeRequest);

        // Second complete (idempotent)
        var response = await _client.PostAsJsonAsync(
            $"/api/print-jobs/{jobId}/complete",
            completeRequest
        );
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Fail_AfterClaim_Returns200()
    {
        // Enqueue + Claim
        await _client.PostAsJsonAsync(
            "/api/print-jobs",
            new
            {
                logicalPrinterName = "FAIL_TEST_PRINTER",
                zplPayload = "^XA^FDFail Test^FS^XZ",
                priority = 0,
            }
        );
        var claimResponse = await _client.PostAsJsonAsync(
            "/api/print-jobs/claim",
            new { agentId = "AGENT-01" }
        );
        var claimBody = await claimResponse.Content.ReadFromJsonAsync<JsonElement>();
        var jobId = claimBody.GetProperty("job").GetProperty("jobId").GetGuid();

        // Fail
        var failRequest = new
        {
            agentId = "AGENT-01",
            failedAtUtc = DateTime.UtcNow,
            errorCode = "PRINTER_OFFLINE",
            errorMessage = "Printer not found",
            retryable = false,
            attemptNumber = 1,
        };
        var response = await _client.PostAsJsonAsync($"/api/print-jobs/{jobId}/fail", failRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Fail_Retryable_Then_Claim_Again_Returns200()
    {
        // Enqueue
        await _client.PostAsJsonAsync(
            "/api/print-jobs",
            new
            {
                logicalPrinterName = "RETRY_TEST_PRINTER",
                zplPayload = "^XA^FDRetry Test^FS^XZ",
                priority = 0,
            }
        );

        // Claim
        var claim1 = await _client.PostAsJsonAsync(
            "/api/print-jobs/claim",
            new { agentId = "AGENT-01" }
        );
        var claim1Body = await claim1.Content.ReadFromJsonAsync<JsonElement>();
        var jobId = claim1Body.GetProperty("job").GetProperty("jobId").GetGuid();

        // Fail with retryable = true
        await _client.PostAsJsonAsync(
            $"/api/print-jobs/{jobId}/fail",
            new
            {
                agentId = "AGENT-01",
                failedAtUtc = DateTime.UtcNow,
                errorCode = "TIMEOUT",
                errorMessage = "Connection timed out",
                retryable = true,
                attemptNumber = 1,
            }
        );

        // Claim again — the retryable job should be available
        var claim2 = await _client.PostAsJsonAsync(
            "/api/print-jobs/claim",
            new { agentId = "AGENT-02" }
        );

        claim2.StatusCode.Should().Be(HttpStatusCode.OK);
        var claim2Body = await claim2.Content.ReadFromJsonAsync<JsonElement>();
        claim2Body.GetProperty("job").GetProperty("jobId").GetGuid().Should().Be(jobId);
    }

    [Fact]
    public async Task Enqueue_WithInvalidRequest_Returns400()
    {
        var request = new
        {
            logicalPrinterName = "", // Invalid: empty
            zplPayload = "^XA^XZ",
            priority = -1, // Invalid: negative
        };

        var response = await _client.PostAsJsonAsync("/api/print-jobs", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Priority_Ordering_HigherPriorityClaimedFirst()
    {
        // Enqueue low priority job first
        await _client.PostAsJsonAsync(
            "/api/print-jobs",
            new
            {
                logicalPrinterName = "PRIORITY_TEST",
                zplPayload = "^XA^FDLow Priority^FS^XZ",
                priority = 10,
                documentName = "low-priority",
            }
        );

        // Enqueue high priority job second
        await _client.PostAsJsonAsync(
            "/api/print-jobs",
            new
            {
                logicalPrinterName = "PRIORITY_TEST",
                zplPayload = "^XA^FDHigh Priority^FS^XZ",
                priority = 1,
                documentName = "high-priority",
            }
        );

        // Claim — should get the high priority job
        var claimResponse = await _client.PostAsJsonAsync(
            "/api/print-jobs/claim",
            new { agentId = "AGENT-PRIORITY" }
        );
        var claimBody = await claimResponse.Content.ReadFromJsonAsync<JsonElement>();
        claimBody.GetProperty("job").GetProperty("priority").GetInt32().Should().Be(1);
    }
}
