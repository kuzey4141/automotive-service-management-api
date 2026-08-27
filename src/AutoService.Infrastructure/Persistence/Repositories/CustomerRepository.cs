using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyList<Customer>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.FirstName)
            .ThenBy(customer => customer.LastName)
            .ToListAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers
            .AsNoTracking()
            .SingleOrDefaultAsync(customer => customer.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
