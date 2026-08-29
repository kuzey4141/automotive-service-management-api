using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;

namespace AutoService.Application.Inventory;

public sealed class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public Task<IReadOnlyList<SparePart>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _inventoryRepository.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyList<SparePart>> GetLowStockAsync(
        CancellationToken cancellationToken = default)
    {
        return _inventoryRepository.GetLowStockAsync(cancellationToken);
    }

    public Task<SparePart?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _inventoryRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<SparePart?> CreateAsync(
        string sku,
        string name,
        decimal unitPrice,
        int initialStock,
        int minimumStockLevel,
        CancellationToken cancellationToken = default)
    {
        var normalizedSku = sku.Trim().ToUpperInvariant();

        if (await _inventoryRepository.SkuExistsAsync(
                normalizedSku,
                cancellationToken: cancellationToken))
        {
            return null;
        }

        var sparePart = new SparePart
        {
            Id = Guid.NewGuid(),
            Sku = normalizedSku,
            Name = name.Trim(),
            UnitPrice = unitPrice,
            QuantityInStock = initialStock,
            MinimumStockLevel = minimumStockLevel
        };

        await _inventoryRepository.AddAsync(sparePart, cancellationToken);
        return sparePart;
    }

    public async Task<SparePart?> UpdateAsync(
        Guid id,
        string sku,
        string name,
        decimal unitPrice,
        int minimumStockLevel,
        CancellationToken cancellationToken = default)
    {
        var sparePart = await _inventoryRepository.GetByIdAsync(id, cancellationToken);

        if (sparePart is null)
        {
            return null;
        }

        var normalizedSku = sku.Trim().ToUpperInvariant();

        if (await _inventoryRepository.SkuExistsAsync(
                normalizedSku,
                id,
                cancellationToken))
        {
            return null;
        }

        sparePart.Sku = normalizedSku;
        sparePart.Name = name.Trim();
        sparePart.UnitPrice = unitPrice;
        sparePart.MinimumStockLevel = minimumStockLevel;
        sparePart.UpdatedAtUtc = DateTime.UtcNow;

        await _inventoryRepository.UpdateAsync(sparePart, cancellationToken);
        return sparePart;
    }

    public async Task<SparePart?> AdjustStockAsync(
        Guid id,
        int quantityChange,
        CancellationToken cancellationToken = default)
    {
        var sparePart = await _inventoryRepository.GetByIdAsync(id, cancellationToken);

        if (sparePart is null || sparePart.QuantityInStock + quantityChange < 0)
        {
            return null;
        }

        sparePart.QuantityInStock += quantityChange;
        sparePart.UpdatedAtUtc = DateTime.UtcNow;

        await _inventoryRepository.UpdateAsync(sparePart, cancellationToken);
        return sparePart;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var sparePart = await _inventoryRepository.GetByIdAsync(id, cancellationToken);
        return sparePart is not null &&
            await _inventoryRepository.DeleteAsync(sparePart, cancellationToken);
    }

    public Task<ServiceRecordPart?> UsePartAsync(
        Guid serviceRecordId,
        Guid sparePartId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        return _inventoryRepository.UsePartAsync(
            serviceRecordId,
            sparePartId,
            quantity,
            cancellationToken);
    }

    public Task<bool> RemovePartUsageAsync(
        Guid serviceRecordId,
        Guid sparePartId,
        CancellationToken cancellationToken = default)
    {
        return _inventoryRepository.RemovePartUsageAsync(
            serviceRecordId,
            sparePartId,
            cancellationToken);
    }

    public Task<IReadOnlyList<ServiceRecordPart>> GetPartsByServiceRecordIdAsync(
        Guid serviceRecordId,
        CancellationToken cancellationToken = default)
    {
        return _inventoryRepository.GetPartsByServiceRecordIdAsync(
            serviceRecordId,
            cancellationToken);
    }
}
