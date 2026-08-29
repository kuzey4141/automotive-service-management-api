using AutoService.Domain.Entities;

namespace AutoService.Application.Appointments;

public interface IServiceAppointmentService
{
    Task<IReadOnlyList<ServiceAppointment>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceAppointment>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default);
    Task<ServiceAppointment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    Task<ServiceAppointment?> CreateAsync(
        Guid vehicleId,
        DateTime scheduledAtUtc,
        string description,
        CancellationToken cancellationToken = default);
    Task<ServiceAppointment?> UpdateAsync(
        Guid id,
        DateTime scheduledAtUtc,
        string description,
        AppointmentStatus status,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
