using LimonikOne.Modules.Product.Application.Products;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.GetAll;

public sealed record GetAllProductsQuery : IQuery<IReadOnlyList<ProductDto>>;
