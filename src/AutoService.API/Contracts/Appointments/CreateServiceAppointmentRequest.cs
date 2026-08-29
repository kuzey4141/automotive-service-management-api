using AutoService.API.Validation;
using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.Appointments;

public sealed class CreateServiceAppointmentRequest
{
    public Guid VehicleId { get; set; }
    [FutureUtcDate]
    public DateTime ScheduledAtUtc { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}
