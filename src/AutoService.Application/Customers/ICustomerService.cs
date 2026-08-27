using AutoService.Domain.Entities;

namespace AutoService.Application.Customers;

public interface ICustomerService
{
    Task<Customer> CreateAsync(
        string firstName,
        string lastName,
        string phoneNumber,
        string? email,
        CancellationToken cancellationToken = default);
}
