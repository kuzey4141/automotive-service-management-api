using AutoService.Application.Abstractions.Persistence;
using AutoService.Application.Appointments;
using AutoService.Domain.Entities;
using NSubstitute;

namespace AutoService.Application.Tests;

public sealed class ServiceAppointmentServiceTests
{
    private readonly IServiceAppointmentRepository _appointmentRepository =
        Substitute.For<IServiceAppointmentRepository>();
    private readonly IVehicleRepository _vehicleRepository =
        Substitute.For<IVehicleRepository>();

    [Fact]
    public async Task CreateAsync_WhenVehicleExists_CreatesPendingAppointment()
    {
        var vehicleId = Guid.NewGuid();
        _vehicleRepository.GetByIdAsync(vehicleId, Arg.Any<CancellationToken>())
            .Returns(new Vehicle { Id = vehicleId });
        var service = new ServiceAppointmentService(
            _appointmentRepository,
            _vehicleRepository);

        var result = await service.CreateAsync(
            vehicleId,
            new DateTime(2027, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            "  Periodic maintenance  ");

        Assert.NotNull(result);
        Assert.Equal(vehicleId, result.VehicleId);
        Assert.Equal("Periodic maintenance", result.Description);
        Assert.Equal(AppointmentStatus.Pending, result.Status);
        await _appointmentRepository.Received(1).AddAsync(
            result,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenVehicleDoesNotExist_ReturnsNull()
    {
        var service = new ServiceAppointmentService(
            _appointmentRepository,
            _vehicleRepository);

        var result = await service.CreateAsync(
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1),
            "Maintenance");

        Assert.Null(result);
        await _appointmentRepository.DidNotReceive().AddAsync(
            Arg.Any<ServiceAppointment>(),
            Arg.Any<CancellationToken>());
    }
}
