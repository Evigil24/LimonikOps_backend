using LimonikOne.Modules.Product.Application.Products;
using LimonikOne.Modules.Product.Domain.Products;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.GetById;

internal sealed class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductDto>> HandleAsync(
        GetProductByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var product = await _productRepository.GetByIdAsync(
            ProductId.From(query.Id),
            cancellationToken
        );

        if (product is null)
        {
            return Result.Failure<ProductDto>(ProductErrors.NotFound(query.Id));
        }

        var dto = new ProductDto(
            product.Id.Value,
            product.DisplayId,
            product.ItemNumber,
            product.PrimaryName,
            product.SearchName,
            product.Variety.Label,
            product.Handling.Label,
            product.Certification.Label,
            product.Stage.Label
        );

        return Result.Success(dto);
    }
}
