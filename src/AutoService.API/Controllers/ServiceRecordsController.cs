using AutoService.API.Contracts.ServiceRecords;
using AutoService.Application.ServiceRecords;
using AutoService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.API.Controllers;

[ApiController]
[Route("api/service-records")]
public sealed class ServiceRecordsController : ControllerBase
{
    private readonly IServiceRecordService _serviceRecordService;

    public ServiceRecordsController(IServiceRecordService serviceRecordService)
    {
        _serviceRecordService = serviceRecordService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ServiceRecordResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var records = await _serviceRecordService.GetAllAsync(cancellationToken);
        return Ok(records.Select(ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceRecordResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var record = await _serviceRecordService.GetByIdAsync(id, cancellationToken);
        return record is null ? NotFound() : Ok(ToResponse(record));
    }

    [HttpGet("/api/vehicles/{vehicleId:guid}/service-records")]
    public async Task<ActionResult<IReadOnlyList<ServiceRecordResponse>>> GetByVehicleId(
        Guid vehicleId,
        CancellationToken cancellationToken)
    {
        var records = await _serviceRecordService.GetByVehicleIdAsync(
            vehicleId,
            cancellationToken);

        return Ok(records.Select(ToResponse).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<ServiceRecordResponse>> Create(
        CreateServiceRecordRequest request,
        CancellationToken cancellationToken)
    {
        if (request.VehicleId == Guid.Empty)
        {
            return BadRequest(new { message = "VehicleId is required." });
        }

        if (request.CompletedAtUtc == default)
        {
            return BadRequest(new { message = "CompletedAtUtc is required." });
        }

        var record = await _serviceRecordService.CreateAsync(
            request.VehicleId,
            request.AppointmentId,
            request.Type,
            request.Odometer,
            request.Description,
            request.LaborCost,
            request.CompletedAtUtc,
            cancellationToken);

        if (record is null)
        {
            return BadRequest(new { message = "Vehicle or appointment is invalid." });
        }

        return CreatedAtAction(nameof(GetById), new { id = record.Id }, ToResponse(record));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ServiceRecordResponse>> Update(
        Guid id,
        UpdateServiceRecordRequest request,
        CancellationToken cancellationToken)
    {
        if (request.CompletedAtUtc == default)
        {
            return BadRequest(new { message = "CompletedAtUtc is required." });
        }

        var record = await _serviceRecordService.UpdateAsync(
            id,
            request.Type,
            request.Odometer,
            request.Description,
            request.LaborCost,
            request.CompletedAtUtc,
            cancellationToken);

        return record is null ? NotFound() : Ok(ToResponse(record));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _serviceRecordService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static ServiceRecordResponse ToResponse(ServiceRecord serviceRecord)
    {
        return new ServiceRecordResponse
        {
            Id = serviceRecord.Id,
            VehicleId = serviceRecord.VehicleId,
            AppointmentId = serviceRecord.AppointmentId,
            Type = serviceRecord.Type,
            Odometer = serviceRecord.Odometer,
            Description = serviceRecord.Description,
            LaborCost = serviceRecord.LaborCost,
            CompletedAtUtc = serviceRecord.CompletedAtUtc,
            CreatedAtUtc = serviceRecord.CreatedAtUtc
        };
    }
}
