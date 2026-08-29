using AutoService.Domain.Entities;

namespace AutoService.Application.Authentication;

public interface IPasswordHashService
{
    string Hash(User user, string password);
    bool Verify(User user, string passwordHash, string password);
}
