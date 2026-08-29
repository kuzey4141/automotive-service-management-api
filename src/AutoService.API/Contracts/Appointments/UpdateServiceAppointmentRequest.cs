using AutoService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.Appointments;

public sealed class UpdateServiceAppointmentRequest
{
    public DateTime ScheduledAtUtc { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public AppointmentStatus Status { get; set; }
}
