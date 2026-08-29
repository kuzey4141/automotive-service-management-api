using AutoService.API.Validation;
using AutoService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.Appointments;

public sealed class UpdateServiceAppointmentRequest
{
    [FutureUtcDate]
    public DateTime ScheduledAtUtc { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [EnumDataType(typeof(AppointmentStatus))]
    public AppointmentStatus Status { get; set; }
}
