using LimonikOne.Modules.Product.Domain.Products;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Products.Create;

internal sealed class CreateProductHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductUnitOfWork _unitOfWork;

    public CreateProductHandler(IProductRepository productRepository, IProductUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> HandleAsync(
        CreateProductCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var exists = await _productRepository.ExistsByItemNumberAsync(
            command.ItemNumber,
            cancellationToken
        );

        if (exists)
        {
            return Result.Failure<Guid>(ProductErrors.DuplicateItemNumber(command.ItemNumber));
        }

        var product = Domain.Products.Product.Create(
            command.ItemNumber,
            command.PrimaryName,
            command.SearchName,
            Variety.FromId(command.VarietyId),
            Handling.FromId(command.HandlingId),
            Certification.FromId(command.CertificationId),
            Stage.FromId(command.StageId)
        );

        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.Id.Value);
    }
}
