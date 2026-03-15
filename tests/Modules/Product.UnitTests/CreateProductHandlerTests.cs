using FluentAssertions;
using LimonikOne.Modules.Product.Application;
using LimonikOne.Modules.Product.Application.Products.Create;
using LimonikOne.Modules.Product.Domain.Products;
using NSubstitute;
using Xunit;
using ProductAggregate = LimonikOne.Modules.Product.Domain.Products.Product;

namespace LimonikOne.Modules.Product.UnitTests;

public class CreateProductHandlerTests
{
    private static CreateProductCommand ValidCommand() =>
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
        var repository = Substitute.For<IProductRepository>();
        var unitOfWork = Substitute.For<IProductUnitOfWork>();
        repository
            .ExistsByItemNumberAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var sut = new CreateProductHandler(repository, unitOfWork);
        var command = ValidCommand();

        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        await repository
            .Received(1)
            .AddAsync(
                Arg.Is<ProductAggregate>(p =>
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
        var repository = Substitute.For<IProductRepository>();
        var unitOfWork = Substitute.For<IProductUnitOfWork>();
        repository
            .ExistsByItemNumberAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var sut = new CreateProductHandler(repository, unitOfWork);
        var command = ValidCommand();

        var result = await sut.HandleAsync(command);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Product.DuplicateItemNumber");
        result.Error.Message.Should().Contain(command.ItemNumber);
        await repository
            .DidNotReceive()
            .AddAsync(Arg.Any<ProductAggregate>(), Arg.Any<CancellationToken>());
        await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
