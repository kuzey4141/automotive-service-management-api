using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.Inventory;

public sealed class CreateSparePartRequest
{
    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "999999999")]
    public decimal UnitPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int InitialStock { get; set; }

    [Range(0, int.MaxValue)]
    public int MinimumStockLevel { get; set; }
}
