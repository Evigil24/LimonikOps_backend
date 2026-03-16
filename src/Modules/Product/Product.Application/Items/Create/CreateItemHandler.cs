using LimonikOne.Modules.Product.Application.Items;
using LimonikOne.Modules.Product.Domain.Items;
using LimonikOne.Shared.Abstractions.Application;

namespace LimonikOne.Modules.Product.Application.Items.Create;

internal sealed class CreateItemHandler(
    IItemRepository itemRepository,
    IProductUnitOfWork unitOfWork
) : ICommandHandler<CreateItemCommand, Guid>
{
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IProductUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(
        CreateItemCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var exists = await _itemRepository.ExistsByItemNumberAsync(
            command.ItemNumber,
            cancellationToken
        );

        if (exists)
        {
            return Result.Failure<Guid>(ItemErrors.DuplicateItemNumber(command.ItemNumber));
        }

        var item = Item.Create(
            command.ItemNumber,
            command.PrimaryName,
            command.SearchName,
            Variety.FromId(command.VarietyId),
            Handling.FromId(command.HandlingId),
            Certification.FromId(command.CertificationId),
            Stage.FromId(command.StageId)
        );

        await _itemRepository.AddAsync(item, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(item.Id.Value);
    }
}
