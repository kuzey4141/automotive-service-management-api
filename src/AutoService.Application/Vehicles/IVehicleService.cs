using AutoService.Domain.Entities;

namespace AutoService.Application.Vehicles;

public interface IVehicleService
{
    Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vehicle>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Vehicle?> CreateAsync(
        Guid customerId,
        string licensePlate,
        string brand,
        string model,
        int modelYear,
        int kilometer,
        CancellationToken cancellationToken = default);
    Task<Vehicle?> UpdateAsync(
        Guid id,
        string licensePlate,
        string brand,
        string model,
        int modelYear,
        int kilometer,
        CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
