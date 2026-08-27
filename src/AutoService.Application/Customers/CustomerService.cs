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
}
