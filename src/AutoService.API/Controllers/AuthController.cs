using AutoService.API.Contracts.Authentication;
using AutoService.Application.Authentication;
using AutoService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("setup-admin")]
    public async Task<ActionResult<AuthResponse>> SetupAdmin(
        SetupAdminRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.SetupAdminAsync(
            request.FullName,
            request.Email,
            request.Password,
            cancellationToken);

        return ToActionResult(result);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(
            request.FullName,
            request.Email,
            request.Password,
            request.Role,
            cancellationToken);

        return ToActionResult(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken);

        return ToActionResult(result, unauthorizedOnFailure: true);
    }

    private ActionResult<AuthResponse> ToActionResult(
        AuthResult result,
        bool unauthorizedOnFailure = false)
    {
        if (!result.Succeeded || result.User is null || result.AccessToken is null)
        {
            return unauthorizedOnFailure
                ? Unauthorized(new { message = result.Error })
                : Conflict(new { message = result.Error });
        }

        return Ok(new AuthResponse
        {
            UserId = result.User.Id,
            FullName = result.User.FullName,
            Email = result.User.Email,
            Role = result.User.Role,
            AccessToken = result.AccessToken.Value,
            ExpiresAtUtc = result.AccessToken.ExpiresAtUtc
        });
    }
}
