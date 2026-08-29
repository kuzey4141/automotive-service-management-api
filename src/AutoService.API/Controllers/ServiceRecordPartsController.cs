using AutoService.API.Contracts.Inventory;
using AutoService.Application.Inventory;
using AutoService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/service-records/{serviceRecordId:guid}/parts")]
public sealed class ServiceRecordPartsController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public ServiceRecordPartsController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ServiceRecordPartResponse>>> GetAll(
        Guid serviceRecordId,
        CancellationToken cancellationToken)
    {
        var parts = await _inventoryService.GetPartsByServiceRecordIdAsync(
            serviceRecordId,
            cancellationToken);

        return Ok(parts.Select(ToResponse).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<ServiceRecordPartResponse>> UsePart(
        Guid serviceRecordId,
        UsePartRequest request,
        CancellationToken cancellationToken)
    {
        if (request.SparePartId == Guid.Empty)
        {
            return BadRequest(new { message = "SparePartId is required." });
        }

        var usage = await _inventoryService.UsePartAsync(
            serviceRecordId,
            request.SparePartId,
            request.Quantity,
            cancellationToken);

        return usage is null
            ? BadRequest(new { message = "Service record, spare part, or stock is invalid." })
            : Ok(ToResponse(usage));
    }

    [HttpDelete("{sparePartId:guid}")]
    public async Task<IActionResult> RemovePart(
        Guid serviceRecordId,
        Guid sparePartId,
        CancellationToken cancellationToken)
    {
        var removed = await _inventoryService.RemovePartUsageAsync(
            serviceRecordId,
            sparePartId,
            cancellationToken);

        return removed ? NoContent() : NotFound();
    }

    private static ServiceRecordPartResponse ToResponse(ServiceRecordPart usage)
    {
        return new ServiceRecordPartResponse
        {
            ServiceRecordId = usage.ServiceRecordId,
            SparePartId = usage.SparePartId,
            Sku = usage.SparePart.Sku,
            Name = usage.SparePart.Name,
            Quantity = usage.Quantity,
            UnitPriceAtUse = usage.UnitPriceAtUse,
            TotalPrice = usage.UnitPriceAtUse * usage.Quantity
        };
    }
}
