using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.Persistence.Repositories;

public sealed class ServiceRecordRepository : IServiceRecordRepository
{
    private readonly AutoServiceDbContext _dbContext;

    public ServiceRecordRepository(AutoServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        ServiceRecord serviceRecord,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ServiceRecords.AddAsync(serviceRecord, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ServiceRecord>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ServiceRecords
            .AsNoTracking()
            .OrderByDescending(serviceRecord => serviceRecord.CompletedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ServiceRecord>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ServiceRecords
            .AsNoTracking()
            .Where(serviceRecord => serviceRecord.VehicleId == vehicleId)
            .OrderByDescending(serviceRecord => serviceRecord.CompletedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<ServiceRecord?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ServiceRecords
            .AsNoTracking()
            .SingleOrDefaultAsync(serviceRecord => serviceRecord.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(
        ServiceRecord serviceRecord,
        CancellationToken cancellationToken = default)
    {
        _dbContext.ServiceRecords.Update(serviceRecord);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        ServiceRecord serviceRecord,
        CancellationToken cancellationToken = default)
    {
        _dbContext.ServiceRecords.Remove(serviceRecord);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
