namespace AutoService.API.Contracts.Inventory;

public sealed class ServiceRecordPartResponse
{
    public Guid ServiceRecordId { get; init; }
    public Guid SparePartId { get; init; }
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPriceAtUse { get; init; }
    public decimal TotalPrice { get; init; }
}
