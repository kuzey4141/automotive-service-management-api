using AutoService.Application.Abstractions.Persistence;
using AutoService.Application.Authentication;
using AutoService.Domain.Entities;
using NSubstitute;

namespace AutoService.Application.Tests;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task SetupAdminAsync_WhenNoUserExists_CreatesAdmin()
    {
        var repository = Substitute.For<IUserRepository>();
        var passwordService = Substitute.For<IPasswordHashService>();
        var tokenProvider = Substitute.For<IAccessTokenProvider>();
        passwordService.Hash(Arg.Any<User>(), "StrongPassword123!").Returns("hashed");
        tokenProvider.Create(Arg.Any<User>())
            .Returns(new AccessToken("token", DateTime.UtcNow.AddHours(1)));
        var service = new AuthService(repository, passwordService, tokenProvider);

        var result = await service.SetupAdminAsync(
            "Test Admin",
            "ADMIN@EXAMPLE.COM",
            "StrongPassword123!");

        Assert.True(result.Succeeded);
        Assert.Equal(UserRole.Admin, result.User!.Role);
        Assert.Equal("admin@example.com", result.User.Email);
        Assert.Equal("hashed", result.User.PasswordHash);
        await repository.Received(1).AddAsync(
            result.User,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SetupAdminAsync_WhenUserExists_ReturnsFailure()
    {
        var repository = Substitute.For<IUserRepository>();
        repository.AnyAsync(Arg.Any<CancellationToken>()).Returns(true);
        var service = new AuthService(
            repository,
            Substitute.For<IPasswordHashService>(),
            Substitute.For<IAccessTokenProvider>());

        var result = await service.SetupAdminAsync("Admin", "admin@example.com", "password");

        Assert.False(result.Succeeded);
        await repository.DidNotReceive().AddAsync(
            Arg.Any<User>(),
            Arg.Any<CancellationToken>());
    }
}
