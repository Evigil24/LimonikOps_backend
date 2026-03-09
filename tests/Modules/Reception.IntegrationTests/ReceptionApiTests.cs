using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LimonikOne.Modules.Reception.Api.Controllers;
using LimonikOne.Modules.Reception.Application.Receptions.Get;

namespace LimonikOne.Modules.Reception.IntegrationTests;

public class ReceptionApiTests : IClassFixture<ReceptionApiFactory>
{
    private readonly HttpClient _client;

    public ReceptionApiTests(ReceptionApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateReception_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new CreateReceptionRequest("John", "Doe", "VIP guest");

        // Act
        var response = await _client.PostAsJsonAsync("/api/receptions", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateReceptionResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task CreateReception_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateReceptionRequest("", "", null);

        // Act
        var response = await _client.PostAsJsonAsync("/api/receptions", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetReceptionById_WhenExists_ReturnsOk()
    {
        // Arrange
        var createRequest = new CreateReceptionRequest("Jane", "Smith", null);
        var createResponse = await _client.PostAsJsonAsync("/api/receptions", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateReceptionResponse>();

        // Act
        var response = await _client.GetAsync($"/api/receptions/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var reception = await response.Content.ReadFromJsonAsync<ReceptionDto>();
        reception.Should().NotBeNull();
        reception!.FirstName.Should().Be("Jane");
        reception.LastName.Should().Be("Smith");
        reception.Status.Should().Be("Pending");
    }

    [Fact]
    public async Task GetReceptionById_WhenNotExists_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/receptions/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
