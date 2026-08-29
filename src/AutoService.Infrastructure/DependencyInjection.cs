using AutoService.Application.Abstractions.Persistence;
using AutoService.Application.Appointments;
using AutoService.Application.Customers;
using AutoService.Application.Inventory;
using AutoService.Application.ServiceRecords;
using AutoService.Application.Vehicles;
using AutoService.Infrastructure.Persistence;
using AutoService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AutoServiceDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IServiceAppointmentRepository, ServiceAppointmentRepository>();
        services.AddScoped<IServiceAppointmentService, ServiceAppointmentService>();
        services.AddScoped<IServiceRecordRepository, ServiceRecordRepository>();
        services.AddScoped<IServiceRecordService, ServiceRecordService>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IInventoryService, InventoryService>();

        return services;
    }
}
