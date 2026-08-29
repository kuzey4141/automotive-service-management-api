namespace AutoService.Domain.Entities;

public class ServiceRecordPart
{
    public Guid ServiceRecordId { get; set; }
    public Guid SparePartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPriceAtUse { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public ServiceRecord ServiceRecord { get; set; } = null!;
    public SparePart SparePart { get; set; } = null!;
}
