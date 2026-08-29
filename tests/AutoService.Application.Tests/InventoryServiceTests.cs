using AutoService.Application.Abstractions.Persistence;
using AutoService.Application.Inventory;
using AutoService.Domain.Entities;
using NSubstitute;

namespace AutoService.Application.Tests;

public sealed class InventoryServiceTests
{
    [Fact]
    public async Task AdjustStockAsync_WhenResultWouldBeNegative_DoesNotUpdatePart()
    {
        var repository = Substitute.For<IInventoryRepository>();
        var partId = Guid.NewGuid();
        repository.GetByIdAsync(partId, Arg.Any<CancellationToken>())
            .Returns(new SparePart { Id = partId, QuantityInStock = 3 });
        var service = new InventoryService(repository);

        var result = await service.AdjustStockAsync(partId, -4);

        Assert.Null(result);
        await repository.DidNotReceive().UpdateAsync(
            Arg.Any<SparePart>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_NormalizesSkuBeforeSaving()
    {
        var repository = Substitute.For<IInventoryRepository>();
        var service = new InventoryService(repository);

        var result = await service.CreateAsync(
            " oil-001 ",
            "Oil Filter",
            350,
            10,
            2);

        Assert.NotNull(result);
        Assert.Equal("OIL-001", result.Sku);
        await repository.Received(1).AddAsync(result, Arg.Any<CancellationToken>());
    }
}
