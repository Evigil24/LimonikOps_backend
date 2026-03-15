using LimonikOne.Modules.Product.Application.Products;
using LimonikOne.Modules.Product.Domain.Products;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.GetAll;

internal sealed class GetAllProductsHandler
    : IQueryHandler<GetAllProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<IReadOnlyList<ProductDto>>> HandleAsync(
        GetAllProductsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        var dtos = products
            .Select(ProductDto.FromEntity)
            .ToList();

        return Result.Success<IReadOnlyList<ProductDto>>(dtos);
    }
}
