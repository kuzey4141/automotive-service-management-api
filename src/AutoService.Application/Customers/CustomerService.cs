using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;

namespace AutoService.Application.Customers;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<IReadOnlyList<Customer>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _customerRepository.GetAllAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _customerRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Customer> CreateAsync(
        string firstName,
        string lastName,
        string phoneNumber,
        string? email,
        CancellationToken cancellationToken = default)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            PhoneNumber = phoneNumber.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim()
        };

        await _customerRepository.AddAsync(customer, cancellationToken);

        return customer;
    }

    public async Task<Customer?> UpdateAsync(
        Guid id,
        string firstName,
        string lastName,
        string phoneNumber,
        string? email,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return null;
        }

        customer.FirstName = firstName.Trim();
        customer.LastName = lastName.Trim();
        customer.PhoneNumber = phoneNumber.Trim();
        customer.Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();

        await _customerRepository.UpdateAsync(customer, cancellationToken);

        return customer;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return false;
        }

        await _customerRepository.DeleteAsync(customer, cancellationToken);

        return true;
    }
}
