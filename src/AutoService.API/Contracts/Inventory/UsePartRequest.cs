using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.Inventory;

public sealed class UsePartRequest
{
    public Guid SparePartId { get; set; }

    [Range(1, 1000000)]
    public int Quantity { get; set; }
}
