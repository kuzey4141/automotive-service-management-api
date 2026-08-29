using AutoService.Domain.Entities;

namespace AutoService.Application.ServiceRecords;

public interface IServiceRecordService
{
    Task<IReadOnlyList<ServiceRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceRecord>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default);
    Task<ServiceRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceRecord?> CreateAsync(
        Guid vehicleId,
        Guid? appointmentId,
        ServiceType type,
        int odometer,
        string description,
        decimal laborCost,
        DateTime completedAtUtc,
        CancellationToken cancellationToken = default);
    Task<ServiceRecord?> UpdateAsync(
        Guid id,
        ServiceType type,
        int odometer,
        string description,
        decimal laborCost,
        DateTime completedAtUtc,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
