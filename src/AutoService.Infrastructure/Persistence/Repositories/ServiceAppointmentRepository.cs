using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.Persistence.Repositories;

public sealed class ServiceAppointmentRepository : IServiceAppointmentRepository
{
    private readonly AutoServiceDbContext _dbContext;

    public ServiceAppointmentRepository(AutoServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        ServiceAppointment appointment,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ServiceAppointments.AddAsync(appointment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ServiceAppointment>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ServiceAppointments
            .AsNoTracking()
            .OrderBy(appointment => appointment.ScheduledAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ServiceAppointment>> GetByVehicleIdAsync(
        Guid vehicleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ServiceAppointments
            .AsNoTracking()
            .Where(appointment => appointment.VehicleId == vehicleId)
            .OrderBy(appointment => appointment.ScheduledAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<ServiceAppointment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ServiceAppointments
            .AsNoTracking()
            .SingleOrDefaultAsync(appointment => appointment.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(
        ServiceAppointment appointment,
        CancellationToken cancellationToken = default)
    {
        _dbContext.ServiceAppointments.Update(appointment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        ServiceAppointment appointment,
        CancellationToken cancellationToken = default)
    {
        _dbContext.ServiceAppointments.Remove(appointment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
