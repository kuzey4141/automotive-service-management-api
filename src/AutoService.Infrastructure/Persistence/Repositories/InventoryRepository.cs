using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.Persistence.Repositories;

public sealed class InventoryRepository : IInventoryRepository
{
    private readonly AutoServiceDbContext _dbContext;

    public InventoryRepository(AutoServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        SparePart sparePart,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SpareParts.AddAsync(sparePart, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SparePart>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SpareParts
            .AsNoTracking()
            .OrderBy(sparePart => sparePart.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SparePart>> GetLowStockAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SpareParts
            .AsNoTracking()
            .Where(sparePart => sparePart.QuantityInStock <= sparePart.MinimumStockLevel)
            .OrderBy(sparePart => sparePart.QuantityInStock)
            .ToListAsync(cancellationToken);
    }

    public Task<SparePart?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.SpareParts
            .AsNoTracking()
            .SingleOrDefaultAsync(sparePart => sparePart.Id == id, cancellationToken);
    }

    public Task<bool> SkuExistsAsync(
        string sku,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.SpareParts.AnyAsync(
            sparePart => sparePart.Sku == sku &&
                (!excludedId.HasValue || sparePart.Id != excludedId.Value),
            cancellationToken);
    }

    public async Task UpdateAsync(
        SparePart sparePart,
        CancellationToken cancellationToken = default)
    {
        _dbContext.SpareParts.Update(sparePart);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        SparePart sparePart,
        CancellationToken cancellationToken = default)
    {
        var hasUsage = await _dbContext.ServiceRecordParts
            .AnyAsync(usage => usage.SparePartId == sparePart.Id, cancellationToken);

        if (hasUsage)
        {
            return false;
        }

        _dbContext.SpareParts.Remove(sparePart);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ServiceRecordPart?> UsePartAsync(
        Guid serviceRecordId,
        Guid sparePartId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(
            cancellationToken);

        var serviceRecordExists = await _dbContext.ServiceRecords
            .AnyAsync(record => record.Id == serviceRecordId, cancellationToken);
        var sparePart = await _dbContext.SpareParts
            .SingleOrDefaultAsync(part => part.Id == sparePartId, cancellationToken);

        if (!serviceRecordExists || sparePart is null || sparePart.QuantityInStock < quantity)
        {
            return null;
        }

        var usage = await _dbContext.ServiceRecordParts
            .Include(item => item.SparePart)
            .SingleOrDefaultAsync(
                item => item.ServiceRecordId == serviceRecordId &&
                    item.SparePartId == sparePartId,
                cancellationToken);

        if (usage is null)
        {
            usage = new ServiceRecordPart
            {
                ServiceRecordId = serviceRecordId,
                SparePartId = sparePartId,
                Quantity = quantity,
                UnitPriceAtUse = sparePart.UnitPrice,
                SparePart = sparePart
            };

            await _dbContext.ServiceRecordParts.AddAsync(usage, cancellationToken);
        }
        else
        {
            usage.Quantity += quantity;
        }

        sparePart.QuantityInStock -= quantity;
        sparePart.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return usage;
    }

    public async Task<bool> RemovePartUsageAsync(
        Guid serviceRecordId,
        Guid sparePartId,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(
            cancellationToken);

        var usage = await _dbContext.ServiceRecordParts.SingleOrDefaultAsync(
            item => item.ServiceRecordId == serviceRecordId && item.SparePartId == sparePartId,
            cancellationToken);

        if (usage is null)
        {
            return false;
        }

        var sparePart = await _dbContext.SpareParts.SingleAsync(
            part => part.Id == sparePartId,
            cancellationToken);

        sparePart.QuantityInStock += usage.Quantity;
        sparePart.UpdatedAtUtc = DateTime.UtcNow;
        _dbContext.ServiceRecordParts.Remove(usage);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    public async Task<IReadOnlyList<ServiceRecordPart>> GetPartsByServiceRecordIdAsync(
        Guid serviceRecordId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ServiceRecordParts
            .AsNoTracking()
            .Include(item => item.SparePart)
            .Where(item => item.ServiceRecordId == serviceRecordId)
            .OrderBy(item => item.SparePart.Name)
            .ToListAsync(cancellationToken);
    }
}
