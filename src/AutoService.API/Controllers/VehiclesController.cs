using AutoService.API.Contracts.Vehicles;
using AutoService.Application.Vehicles;
using AutoService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/vehicles")]
public sealed class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleService.GetAllAsync(cancellationToken);
        return Ok(vehicles.Select(ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VehicleResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id, cancellationToken);
        return vehicle is null ? NotFound() : Ok(ToResponse(vehicle));
    }

    [HttpGet("/api/customers/{customerId:guid}/vehicles")]
    public async Task<ActionResult<IReadOnlyList<VehicleResponse>>> GetByCustomerId(
        Guid customerId,
        CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleService.GetByCustomerIdAsync(customerId, cancellationToken);
        return Ok(vehicles.Select(ToResponse).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<VehicleResponse>> Create(
        CreateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        if (request.CustomerId == Guid.Empty)
        {
            return BadRequest(new { message = "CustomerId is required." });
        }

        var vehicle = await _vehicleService.CreateAsync(
            request.CustomerId,
            request.LicensePlate,
            request.Brand,
            request.Model,
            request.ModelYear,
            request.Kilometer,
            cancellationToken);

        if (vehicle is null)
        {
            return BadRequest(new { message = "Customer not found." });
        }

        return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, ToResponse(vehicle));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VehicleResponse>> Update(
        Guid id,
        UpdateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleService.UpdateAsync(
            id,
            request.LicensePlate,
            request.Brand,
            request.Model,
            request.ModelYear,
            request.Kilometer,
            cancellationToken);

        return vehicle is null ? NotFound() : Ok(ToResponse(vehicle));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var deleted = await _vehicleService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static VehicleResponse ToResponse(Vehicle vehicle)
    {
        return new VehicleResponse
        {
            Id = vehicle.Id,
            CustomerId = vehicle.CustomerId,
            LicensePlate = vehicle.LicensePlate,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            ModelYear = vehicle.ModelYear,
            Kilometer = vehicle.Kilometer,
            CreatedAtUtc = vehicle.CreatedAtUtc
        };
    }
}
