namespace AutoService.API.Contracts.Vehicles;

public sealed class VehicleResponse
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string LicensePlate { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public int ModelYear { get; init; }
    public int Kilometer { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
