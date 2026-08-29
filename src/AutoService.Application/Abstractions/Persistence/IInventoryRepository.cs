using AutoService.Domain.Entities;

namespace AutoService.Application.Abstractions.Persistence;

public interface IInventoryRepository
{
    Task AddAsync(SparePart sparePart, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SparePart>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SparePart>> GetLowStockAsync(CancellationToken cancellationToken = default);
    Task<SparePart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SkuExistsAsync(
        string sku,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(SparePart sparePart, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(SparePart sparePart, CancellationToken cancellationToken = default);
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
