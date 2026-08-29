using AutoService.Application.Abstractions.Persistence;
using AutoService.Domain.Entities;

namespace AutoService.Application.Authentication;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IAccessTokenProvider _accessTokenProvider;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IAccessTokenProvider accessTokenProvider)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<AuthResult> SetupAdminAsync(
        string fullName,
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (await _userRepository.AnyAsync(cancellationToken))
        {
            return AuthResult.Failure("Initial admin setup has already been completed.");
        }

        return await CreateUserAsync(
            fullName,
            email,
            password,
            UserRole.Admin,
            cancellationToken);
    }

    public Task<AuthResult> RegisterAsync(
        string fullName,
        string email,
        string password,
        UserRole role,
        CancellationToken cancellationToken = default)
    {
        return CreateUserAsync(fullName, email, password, role, cancellationToken);
    }

    public async Task<AuthResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null || !_passwordHashService.Verify(user, user.PasswordHash, password))
        {
            return AuthResult.Failure("Email or password is incorrect.");
        }

        return AuthResult.Success(user, _accessTokenProvider.Create(user));
    }

    private async Task<AuthResult> CreateUserAsync(
        string fullName,
        string email,
        string password,
        UserRole role,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (await _userRepository.EmailExistsAsync(normalizedEmail, cancellationToken))
        {
            return AuthResult.Failure("A user with this email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = fullName.Trim(),
            Email = normalizedEmail,
            Role = role
        };

        user.PasswordHash = _passwordHashService.Hash(user, password);
        await _userRepository.AddAsync(user, cancellationToken);

        return AuthResult.Success(user, _accessTokenProvider.Create(user));
    }
}
