using AutoService.Domain.Entities;

namespace AutoService.Application.Inventory;

public interface IInventoryService
{
    Task<IReadOnlyList<SparePart>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SparePart>> GetLowStockAsync(CancellationToken cancellationToken = default);
    Task<SparePart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SparePart?> CreateAsync(
        string sku,
        string name,
        decimal unitPrice,
        int initialStock,
        int minimumStockLevel,
        CancellationToken cancellationToken = default);
    Task<SparePart?> UpdateAsync(
        Guid id,
        string sku,
        string name,
        decimal unitPrice,
        int minimumStockLevel,
        CancellationToken cancellationToken = default);
    Task<SparePart?> AdjustStockAsync(
        Guid id,
        int quantityChange,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceRecordPart?> UsePartAsync(
        Guid serviceRecordId,
        Guid sparePartId,
        int quantity,
        CancellationToken cancellationToken = default);
    Task<bool> RemovePartUsageAsync(
        Guid serviceRecordId,
        Guid sparePartId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceRecordPart>> GetPartsByServiceRecordIdAsync(
        Guid serviceRecordId,
        CancellationToken cancellationToken = default);
}
