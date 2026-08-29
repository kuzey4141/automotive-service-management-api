using AutoService.API.Contracts.Inventory;
using AutoService.Application.Inventory;
using AutoService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.API.Controllers;

[ApiController]
[Route("api/spare-parts")]
public sealed class SparePartsController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public SparePartsController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SparePartResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var parts = await _inventoryService.GetAllAsync(cancellationToken);
        return Ok(parts.Select(ToResponse).ToList());
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IReadOnlyList<SparePartResponse>>> GetLowStock(
        CancellationToken cancellationToken)
    {
        var parts = await _inventoryService.GetLowStockAsync(cancellationToken);
        return Ok(parts.Select(ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SparePartResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var part = await _inventoryService.GetByIdAsync(id, cancellationToken);
        return part is null ? NotFound() : Ok(ToResponse(part));
    }

    [HttpPost]
    public async Task<ActionResult<SparePartResponse>> Create(
        CreateSparePartRequest request,
        CancellationToken cancellationToken)
    {
        var part = await _inventoryService.CreateAsync(
            request.Sku,
            request.Name,
            request.UnitPrice,
            request.InitialStock,
            request.MinimumStockLevel,
            cancellationToken);

        if (part is null)
        {
            return Conflict(new { message = "A spare part with this SKU already exists." });
        }

        return CreatedAtAction(nameof(GetById), new { id = part.Id }, ToResponse(part));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SparePartResponse>> Update(
        Guid id,
        UpdateSparePartRequest request,
        CancellationToken cancellationToken)
    {
        var part = await _inventoryService.UpdateAsync(
            id,
            request.Sku,
            request.Name,
            request.UnitPrice,
            request.MinimumStockLevel,
            cancellationToken);

        return part is null
            ? Conflict(new { message = "Spare part was not found or SKU is already in use." })
            : Ok(ToResponse(part));
    }

    [HttpPost("{id:guid}/stock-adjustments")]
    public async Task<ActionResult<SparePartResponse>> AdjustStock(
        Guid id,
        AdjustStockRequest request,
        CancellationToken cancellationToken)
    {
        if (request.QuantityChange == 0)
        {
            return BadRequest(new { message = "QuantityChange cannot be zero." });
        }

        var part = await _inventoryService.AdjustStockAsync(
            id,
            request.QuantityChange,
            cancellationToken);

        return part is null
            ? BadRequest(new { message = "Spare part was not found or stock cannot be negative." })
            : Ok(ToResponse(part));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _inventoryService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : Conflict(new { message = "Part was not found or has usage history." });
    }

    private static SparePartResponse ToResponse(SparePart sparePart)
    {
        return new SparePartResponse
        {
            Id = sparePart.Id,
            Sku = sparePart.Sku,
            Name = sparePart.Name,
            UnitPrice = sparePart.UnitPrice,
            QuantityInStock = sparePart.QuantityInStock,
            MinimumStockLevel = sparePart.MinimumStockLevel,
            IsLowStock = sparePart.QuantityInStock <= sparePart.MinimumStockLevel,
            CreatedAtUtc = sparePart.CreatedAtUtc,
            UpdatedAtUtc = sparePart.UpdatedAtUtc
        };
    }
}
