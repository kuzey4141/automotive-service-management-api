using AutoService.Domain.Entities;

namespace AutoService.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
