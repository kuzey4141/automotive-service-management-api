namespace AutoService.Domain.Entities;

public class ServiceAppointment
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime ScheduledAtUtc { get; set; }
    public string Description { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Vehicle Vehicle { get; set; } = null!;
    public ServiceRecord? ServiceRecord { get; set; }
}
