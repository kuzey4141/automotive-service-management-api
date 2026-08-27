using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;

namespace AutoService.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AutoServiceDbContext _dbContext;

    public CustomerRepository(AutoServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Customers.AddAsync(customer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
