using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.Vehicles;

public sealed class CreateVehicleRequest
{
    public Guid CustomerId { get; set; }

    [Required]
    [MaxLength(20)]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    [Range(1886, 2100)]
    public int ModelYear { get; set; }

    [Range(0, int.MaxValue)]
    public int Kilometer { get; set; }
}
