using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;

namespace AutoService.Application.Vehicles;

public sealed class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ICustomerRepository _customerRepository;

    public VehicleService(
        IVehicleRepository vehicleRepository,
        ICustomerRepository customerRepository)
    {
        _vehicleRepository = vehicleRepository;
        _customerRepository = customerRepository;
    }

    public Task<IReadOnlyList<Vehicle>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _vehicleRepository.GetAllAsync(cancellationToken);
    }

    public Task<IReadOnlyList<Vehicle>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return _vehicleRepository.GetByCustomerIdAsync(customerId, cancellationToken);
    }

    public Task<Vehicle?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _vehicleRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Vehicle?> CreateAsync(
        Guid customerId,
        string licensePlate,
        string brand,
        string model,
        int modelYear,
        int kilometer,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);

        if (customer is null)
        {
            return null;
        }

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            LicensePlate = licensePlate.Trim().ToUpperInvariant(),
            Brand = brand.Trim(),
            Model = model.Trim(),
            ModelYear = modelYear,
            Kilometer = kilometer
        };

        await _vehicleRepository.AddAsync(vehicle, cancellationToken);

        return vehicle;
    }

    public async Task<Vehicle?> UpdateAsync(
        Guid id,
        string licensePlate,
        string brand,
        string model,
        int modelYear,
        int kilometer,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);

        if (vehicle is null)
        {
            return null;
        }

        vehicle.LicensePlate = licensePlate.Trim().ToUpperInvariant();
        vehicle.Brand = brand.Trim();
        vehicle.Model = model.Trim();
        vehicle.ModelYear = modelYear;
        vehicle.Kilometer = kilometer;

        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

        return vehicle;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);

        if (vehicle is null)
        {
            return false;
        }

        await _vehicleRepository.DeleteAsync(vehicle, cancellationToken);

        return true;
    }
}
