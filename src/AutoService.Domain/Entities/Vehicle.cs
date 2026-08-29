namespace AutoService.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int ModelYear { get; set; }
    public int Kilometer { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Customer Customer { get; set; } = null!;
    public ICollection<ServiceAppointment> Appointments { get; set; } = new List<ServiceAppointment>();
}
