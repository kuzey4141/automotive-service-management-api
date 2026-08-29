using AutoService.API.Contracts.Appointments;
using AutoService.Application.Appointments;
using AutoService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/appointments")]
public sealed class ServiceAppointmentsController : ControllerBase
{
    private readonly IServiceAppointmentService _appointmentService;

    public ServiceAppointmentsController(IServiceAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ServiceAppointmentResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAllAsync(cancellationToken);
        return Ok(appointments.Select(ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceAppointmentResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.GetByIdAsync(id, cancellationToken);
        return appointment is null ? NotFound() : Ok(ToResponse(appointment));
    }

    [HttpGet("/api/vehicles/{vehicleId:guid}/appointments")]
    public async Task<ActionResult<IReadOnlyList<ServiceAppointmentResponse>>> GetByVehicleId(
        Guid vehicleId,
        CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetByVehicleIdAsync(
            vehicleId,
            cancellationToken);

        return Ok(appointments.Select(ToResponse).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<ServiceAppointmentResponse>> Create(
        CreateServiceAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.VehicleId == Guid.Empty)
        {
            return BadRequest(new { message = "VehicleId is required." });
        }

        if (request.ScheduledAtUtc == default)
        {
            return BadRequest(new { message = "ScheduledAtUtc is required." });
        }

        var appointment = await _appointmentService.CreateAsync(
            request.VehicleId,
            request.ScheduledAtUtc,
            request.Description,
            cancellationToken);

        if (appointment is null)
        {
            return BadRequest(new { message = "Vehicle not found." });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = appointment.Id },
            ToResponse(appointment));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ServiceAppointmentResponse>> Update(
        Guid id,
        UpdateServiceAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ScheduledAtUtc == default)
        {
            return BadRequest(new { message = "ScheduledAtUtc is required." });
        }

        var appointment = await _appointmentService.UpdateAsync(
            id,
            request.ScheduledAtUtc,
            request.Description,
            request.Status,
            cancellationToken);

        return appointment is null ? NotFound() : Ok(ToResponse(appointment));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var deleted = await _appointmentService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static ServiceAppointmentResponse ToResponse(ServiceAppointment appointment)
    {
        return new ServiceAppointmentResponse
        {
            Id = appointment.Id,
            VehicleId = appointment.VehicleId,
            ScheduledAtUtc = appointment.ScheduledAtUtc,
            Description = appointment.Description,
            Status = appointment.Status,
            CreatedAtUtc = appointment.CreatedAtUtc
        };
    }
}
