namespace AutoService.Domain.Entities;

public class ServiceRecord
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? AppointmentId { get; set; }
    public ServiceType Type { get; set; }
    public int Odometer { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal LaborCost { get; set; }
    public DateTime CompletedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Vehicle Vehicle { get; set; } = null!;
    public ServiceAppointment? Appointment { get; set; }
    public ICollection<ServiceRecordPart> Parts { get; set; } = new List<ServiceRecordPart>();
}
