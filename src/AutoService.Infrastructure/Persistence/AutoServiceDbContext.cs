using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.Persistence;

public class AutoServiceDbContext : DbContext
{
    public AutoServiceDbContext(DbContextOptions<AutoServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<ServiceAppointment> ServiceAppointments => Set<ServiceAppointment>();
}
