namespace AutoService.API.Contracts.Inventory;

public sealed class SparePartResponse
{
    public Guid Id { get; init; }
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int QuantityInStock { get; init; }
    public int MinimumStockLevel { get; init; }
    public bool IsLowStock { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
