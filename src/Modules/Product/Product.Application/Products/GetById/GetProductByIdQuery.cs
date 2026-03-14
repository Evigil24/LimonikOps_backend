using LimonikOne.Modules.Product.Application.Products;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.GetById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
