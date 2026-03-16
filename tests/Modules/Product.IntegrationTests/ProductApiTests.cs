using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using LimonikOne.Modules.Product.Api.Controllers.Items.Requests;

namespace LimonikOne.Modules.Product.IntegrationTests;

public class ProductApiTests(ProductApiFactory factory) : IClassFixture<ProductApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_Products_With_Valid_Request_Returns_Created()
    {
        var request = new CreateItemRequest(
            ItemNumber: $"ITEM-{Guid.NewGuid():N}"[..20],
            PrimaryName: "Lemon Bulk",
            SearchName: "lemon bulk",
            VarietyId: 1,
            HandlingId: 1,
            CertificationId: 1,
            StageId: 1
        );

        var response = await _client.PostAsJsonAsync("/api/product/items", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CreateItemResponse>();
        body.Should().NotBeNull();
        body!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Post_Products_Duplicate_ItemNumber_Returns_Conflict()
    {
        var itemNumber = $"DUP-{Guid.NewGuid():N}"[..20];
        var request = new CreateItemRequest(itemNumber, "Lemon Bulk", "lemon bulk", 1, 1, 1, 1);

        var first = await _client.PostAsJsonAsync("/api/product/items", request);
        first.StatusCode.Should().Be(HttpStatusCode.Created);

        var second = await _client.PostAsJsonAsync("/api/product/items", request);
        second.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Get_Products_Returns_Ok_And_List()
    {
        var response = await _client.GetAsync("/api/product/items");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<ItemListDto[]>();
        items.Should().NotBeNull();
        items!.Should().BeAssignableTo<IReadOnlyList<object>>();
    }

    [Fact]
    public async Task Post_Then_Get_Products_Returns_Created_Product_In_List()
    {
        var itemNumber = $"GET-{Guid.NewGuid():N}"[..20];
        var request = new CreateItemRequest(itemNumber, "Lemon Sorted", "lemon sorted", 2, 2, 2, 2);

        var createResponse = await _client.PostAsJsonAsync("/api/product/items", request);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateItemResponse>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBeEmpty();

        var listResponse = await _client.GetAsync("/api/product/items");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await listResponse.Content.ReadFromJsonAsync<ItemListDto[]>();
        items.Should().Contain(p => p.ItemNumber == itemNumber && p.Id == created.Id);
    }

    private sealed record CreateItemResponse([property: JsonPropertyName("id")] Guid Id);

    private sealed record ItemListDto(
        Guid Id,
        long DisplayId,
        string ItemNumber,
        string PrimaryName,
        string SearchName,
        string Variety,
        string Handling,
        string Certification,
        string Stage
    );
}
