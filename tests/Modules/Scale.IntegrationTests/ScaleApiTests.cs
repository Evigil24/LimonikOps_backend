using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LimonikOne.Modules.Scale.Api.Controllers.WeightBatches.Requests;

namespace LimonikOne.Modules.Scale.IntegrationTests;

public class ScaleApiTests : IClassFixture<ScaleApiFactory>
{
    private readonly HttpClient _client;

    public ScaleApiTests(ScaleApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task IngestWeightBatch_WithValidRequest_ReturnsOk()
    {
        var request = new IngestWeightBatchRequest(
            Guid.NewGuid(),
            "scale-001",
            "Warehouse A",
            DateTime.UtcNow,
            new List<IngestWeightReadingRequest>
            {
                new(150.5m, 10, DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow, 8),
            }
        );

        var response = await _client.PostAsJsonAsync("/api/weight-batches", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task IngestWeightBatch_WithInvalidRequest_ReturnsBadRequest()
    {
        var request = new IngestWeightBatchRequest(
            Guid.Empty,
            "",
            "",
            default,
            new List<IngestWeightReadingRequest>()
        );

        var response = await _client.PostAsJsonAsync("/api/weight-batches", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task IngestWeightBatch_DuplicateBatchId_ReturnsOk()
    {
        var batchId = Guid.NewGuid();
        var request = new IngestWeightBatchRequest(
            batchId,
            "scale-001",
            "Warehouse A",
            DateTime.UtcNow,
            new List<IngestWeightReadingRequest>
            {
                new(100m, 5, DateTime.UtcNow.AddMinutes(-2), DateTime.UtcNow, 5),
            }
        );

        var firstResponse = await _client.PostAsJsonAsync("/api/weight-batches", request);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondResponse = await _client.PostAsJsonAsync("/api/weight-batches", request);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
