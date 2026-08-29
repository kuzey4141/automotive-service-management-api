using AutoService.Domain.Entities;

namespace AutoService.API.Contracts.Appointments;

public sealed class ServiceAppointmentResponse
{
    public Guid Id { get; init; }
    public Guid VehicleId { get; init; }
    public DateTime ScheduledAtUtc { get; init; }
    public string Description { get; init; } = string.Empty;
    public AppointmentStatus Status { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
