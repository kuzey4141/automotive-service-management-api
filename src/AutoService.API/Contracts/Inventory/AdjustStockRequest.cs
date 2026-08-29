using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.Inventory;

public sealed class AdjustStockRequest
{
    [Range(-1000000, 1000000)]
    public int QuantityChange { get; set; }
}
