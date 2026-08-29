namespace AutoService.Domain.Entities;

public class SparePart
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int QuantityInStock { get; set; }
    public int MinimumStockLevel { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public ICollection<ServiceRecordPart> ServiceRecordParts { get; set; } = new List<ServiceRecordPart>();
}
