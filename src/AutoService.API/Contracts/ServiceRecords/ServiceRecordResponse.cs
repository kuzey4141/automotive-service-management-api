using AutoService.Domain.Entities;

namespace AutoService.API.Contracts.ServiceRecords;

public sealed class ServiceRecordResponse
{
    public Guid Id { get; init; }
    public Guid VehicleId { get; init; }
    public Guid? AppointmentId { get; init; }
    public ServiceType Type { get; init; }
    public int Odometer { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal LaborCost { get; init; }
    public DateTime CompletedAtUtc { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
