using FluentAssertions;
using LimonikOne.Modules.Product.Application;
using LimonikOne.Modules.Product.Application.Items.Create;
using LimonikOne.Modules.Product.Domain.Items;
using NSubstitute;
using Xunit;

namespace LimonikOne.Modules.Product.UnitTests;

public class CreateItemHandlerTests
{
    private static CreateItemCommand ValidCommand() =>
        new(
            ItemNumber: "ITEM-001",
            PrimaryName: "Lemon Bulk",
            SearchName: "lemon bulk",
            VarietyId: 1,
            HandlingId: 1,
            CertificationId: 1,
            StageId: 1
        );

    [Fact]
    public async Task HandleAsync_When_ItemNumber_Not_Taken_Returns_Success_And_Calls_Add_And_SaveChanges()
    {
        var repository = Substitute.For<IItemRepository>();
        var unitOfWork = Substitute.For<IProductUnitOfWork>();
        repository
            .ExistsByItemNumberAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var sut = new CreateItemHandler(repository, unitOfWork);
        var command = ValidCommand();

        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        await repository
            .Received(1)
            .AddAsync(
                Arg.Is<Item>(p =>
                    p.ItemNumber == command.ItemNumber
                    && p.PrimaryName == command.PrimaryName
                    && p.SearchName == command.SearchName
                ),
                Arg.Any<CancellationToken>()
            );
        await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_When_ItemNumber_Already_Exists_Returns_Failure_And_Does_Not_Add()
    {
        var repository = Substitute.For<IItemRepository>();
        var unitOfWork = Substitute.For<IProductUnitOfWork>();
        repository
            .ExistsByItemNumberAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var sut = new CreateItemHandler(repository, unitOfWork);
        var command = ValidCommand();

        var result = await sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Item.DuplicateItemNumber");
        result.Error.Message.Should().Contain(command.ItemNumber);
        await repository.DidNotReceive().AddAsync(Arg.Any<Item>(), Arg.Any<CancellationToken>());
        await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
