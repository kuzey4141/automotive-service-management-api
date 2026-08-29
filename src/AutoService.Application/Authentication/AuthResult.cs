using AutoService.Domain.Entities;

namespace AutoService.Application.Authentication;

public sealed record AuthResult(
    bool Succeeded,
    string? Error,
    User? User,
    AccessToken? AccessToken)
{
    public static AuthResult Failure(string error) => new(false, error, null, null);

    public static AuthResult Success(User user, AccessToken accessToken) =>
        new(true, null, user, accessToken);
}
