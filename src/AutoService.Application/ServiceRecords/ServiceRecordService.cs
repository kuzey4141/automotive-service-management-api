using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;

namespace AutoService.Application.ServiceRecords;

public sealed class ServiceRecordService : IServiceRecordService
{
    private readonly IServiceRecordRepository _serviceRecordRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IServiceAppointmentRepository _appointmentRepository;

    public ServiceRecordService(
        IServiceRecordRepository serviceRecordRepository,
        IVehicleRepository vehicleRepository,
        IServiceAppointmentRepository appointmentRepository)
    {
        _serviceRecordRepository = serviceRecordRepository;
        _vehicleRepository = vehicleRepository;
        _appointmentRepository = appointmentRepository;
    }

    public Task<IReadOnlyList<ServiceRecord>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _serviceRecordRepository.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyList<ServiceRecord>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        return _serviceRecordRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);
    }

    public Task<ServiceRecord?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _serviceRecordRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<ServiceRecord?> CreateAsync(
        Guid vehicleId,
        Guid? appointmentId,
        ServiceType type,
        int odometer,
        string description,
        decimal laborCost,
        DateTime completedAtUtc,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken);

        if (vehicle is null)
        {
            return null;
        }

        if (appointmentId.HasValue)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(
                appointmentId.Value,
                cancellationToken);

            if (appointment is null || appointment.VehicleId != vehicleId)
            {
                return null;
            }
        }

        var serviceRecord = new ServiceRecord
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicleId,
            AppointmentId = appointmentId,
            Type = type,
            Odometer = odometer,
            Description = description.Trim(),
            LaborCost = laborCost,
            CompletedAtUtc = NormalizeUtc(completedAtUtc)
        };

        await _serviceRecordRepository.AddAsync(serviceRecord, cancellationToken);

        return serviceRecord;
    }

    public async Task<ServiceRecord?> UpdateAsync(
        Guid id,
        ServiceType type,
        int odometer,
        string description,
        decimal laborCost,
        DateTime completedAtUtc,
        CancellationToken cancellationToken = default)
    {
        var serviceRecord = await _serviceRecordRepository.GetByIdAsync(id, cancellationToken);

        if (serviceRecord is null)
        {
            return null;
        }

        serviceRecord.Type = type;
        serviceRecord.Odometer = odometer;
        serviceRecord.Description = description.Trim();
        serviceRecord.LaborCost = laborCost;
        serviceRecord.CompletedAtUtc = NormalizeUtc(completedAtUtc);

        await _serviceRecordRepository.UpdateAsync(serviceRecord, cancellationToken);

        return serviceRecord;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var serviceRecord = await _serviceRecordRepository.GetByIdAsync(id, cancellationToken);

        if (serviceRecord is null)
        {
            return false;
        }

        await _serviceRecordRepository.DeleteAsync(serviceRecord, cancellationToken);
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
