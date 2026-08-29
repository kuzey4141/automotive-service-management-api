using AutoService.Domain.Entities;

namespace AutoService.Application.Abstractions.Persistence;

public interface IServiceRecordRepository
{
    Task AddAsync(ServiceRecord serviceRecord, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceRecord>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default);
    Task<ServiceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(ServiceRecord serviceRecord, CancellationToken cancellationToken = default);
    Task DeleteAsync(ServiceRecord serviceRecord, CancellationToken cancellationToken = default);
}
