using AutoService.Domain.Entities;

namespace AutoService.Application.Abstractions.Persistence;

public interface IServiceAppointmentRepository
{
    Task AddAsync(
        ServiceAppointment appointment,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceAppointment>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceAppointment>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default);
    Task<ServiceAppointment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(
        ServiceAppointment appointment,
        CancellationToken cancellationToken = default);
    Task DeleteAsync(
        ServiceAppointment appointment,
        CancellationToken cancellationToken = default);
}
