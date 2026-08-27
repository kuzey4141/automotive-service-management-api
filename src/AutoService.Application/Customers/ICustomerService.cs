using AutoService.Domain.Entities;

namespace AutoService.Application.Customers;

public interface ICustomerService
{
    Task<IReadOnlyList<Customer>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Customer?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Customer> CreateAsync(
        string firstName,
        string lastName,
        string phoneNumber,
        string? email,
        CancellationToken cancellationToken = default);

    Task<Customer?> UpdateAsync(
        Guid id,
        string firstName,
        string lastName,
        string phoneNumber,
        string? email,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
