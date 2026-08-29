using AutoService.API.Validation;
using AutoService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.ServiceRecords;

public sealed class UpdateServiceRecordRequest
{
    [EnumDataType(typeof(ServiceType))]
    public ServiceType Type { get; set; }

    [Range(0, int.MaxValue)]
    public int Odometer { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "999999999")]
    public decimal LaborCost { get; set; }

    [NotFutureUtcDate]
    public DateTime CompletedAtUtc { get; set; }
}
