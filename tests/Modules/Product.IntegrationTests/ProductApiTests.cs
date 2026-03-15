using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using LimonikOne.Modules.Product.Api.Controllers.Products.Requests;

namespace LimonikOne.Modules.Product.IntegrationTests;

public class ProductApiTests : IClassFixture<ProductApiFactory>
{
    private readonly HttpClient _client;

    public ProductApiTests(ProductApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Products_With_Valid_Request_Returns_Created()
    {
        var request = new CreateProductRequest(
            ItemNumber: $"ITEM-{Guid.NewGuid():N}"[..20],
            PrimaryName: "Lemon Bulk",
            SearchName: "lemon bulk",
            VarietyId: 1,
            HandlingId: 1,
            CertificationId: 1,
            StageId: 1
        );

        var response = await _client.PostAsJsonAsync("/api/product/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
        body.Should().NotBeNull();
        body!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Post_Products_Duplicate_ItemNumber_Returns_Conflict()
    {
        var itemNumber = $"DUP-{Guid.NewGuid():N}"[..20];
        var request = new CreateProductRequest(itemNumber, "Lemon Bulk", "lemon bulk", 1, 1, 1, 1);

        var first = await _client.PostAsJsonAsync("/api/product/products", request);
        first.StatusCode.Should().Be(HttpStatusCode.Created);

        var second = await _client.PostAsJsonAsync("/api/product/products", request);
        second.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Get_Products_Returns_Ok_And_List()
    {
        var response = await _client.GetAsync("/api/product/products");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<ProductListDto[]>();
        products.Should().NotBeNull();
        products!.Should().BeAssignableTo<IReadOnlyList<object>>();
    }

    [Fact]
    public async Task Post_Then_Get_Products_Returns_Created_Product_In_List()
    {
        var itemNumber = $"GET-{Guid.NewGuid():N}"[..20];
        var request = new CreateProductRequest(
            itemNumber,
            "Lemon Sorted",
            "lemon sorted",
            2,
            2,
            2,
            2
        );

        var createResponse = await _client.PostAsJsonAsync("/api/product/products", request);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();
        created.Should().NotBeNull();
        created!.Id.Should().NotBeEmpty();

        var listResponse = await _client.GetAsync("/api/product/products");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var products = await listResponse.Content.ReadFromJsonAsync<ProductListDto[]>();
        products.Should().Contain(p => p.ItemNumber == itemNumber && p.Id == created.Id);
    }

    private sealed record CreateProductResponse([property: JsonPropertyName("id")] Guid Id);

    private sealed record ProductListDto(
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
