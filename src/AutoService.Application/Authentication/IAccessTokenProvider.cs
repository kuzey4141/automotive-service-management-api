using AutoService.Domain.Entities;

namespace AutoService.Application.Authentication;

public interface IAccessTokenProvider
{
    AccessToken Create(User user);
}
