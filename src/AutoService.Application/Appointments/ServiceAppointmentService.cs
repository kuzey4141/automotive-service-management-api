using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;

namespace AutoService.Application.Appointments;

public sealed class ServiceAppointmentService : IServiceAppointmentService
{
    private readonly IServiceAppointmentRepository _appointmentRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public ServiceAppointmentService(
        IServiceAppointmentRepository appointmentRepository,
        IVehicleRepository vehicleRepository)
    {
        _appointmentRepository = appointmentRepository;
        _vehicleRepository = vehicleRepository;
    }

    public Task<IReadOnlyList<ServiceAppointment>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _appointmentRepository.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyList<ServiceAppointment>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        return _appointmentRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);
    }

    public Task<ServiceAppointment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _appointmentRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<ServiceAppointment?> CreateAsync(
        Guid vehicleId,
        DateTime scheduledAtUtc,
        string description,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);

        if (vehicle is null)
        {
            return null;
        }

        var appointment = new ServiceAppointment
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicleId,
            ScheduledAtUtc = NormalizeUtc(scheduledAtUtc),
            Description = description.Trim(),
            Status = AppointmentStatus.Pending
        };

        await _appointmentRepository.AddAsync(appointment, cancellationToken);

        return appointment;
    }

    public async Task<ServiceAppointment?> UpdateAsync(
        Guid id,
        DateTime scheduledAtUtc,
        string description,
        AppointmentStatus status,
        CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

        if (appointment is null)
        {
            return null;
        }

        appointment.ScheduledAtUtc = NormalizeUtc(scheduledAtUtc);
        appointment.Description = description.Trim();
        appointment.Status = status;

        await _appointmentRepository.UpdateAsync(appointment, cancellationToken);

        return appointment;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

        if (appointment is null)
        {
            return false;
        }

        await _appointmentRepository.DeleteAsync(appointment, cancellationToken);

        return true;
    }

    private static DateTime NormalizeUtc(DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
    }
}
