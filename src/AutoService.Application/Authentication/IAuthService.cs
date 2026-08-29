using AutoService.Domain.Entities;

namespace AutoService.Application.Authentication;

public interface IAuthService
{
    Task<AuthResult> SetupAdminAsync(
        string fullName,
        string email,
        string password,
        CancellationToken cancellationToken = default);
    Task<AuthResult> RegisterAsync(
        string fullName,
        string email,
        string password,
        UserRole role,
        CancellationToken cancellationToken = default);
    Task<AuthResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}
