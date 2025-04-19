using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Services;

public interface IAuthTokenService
{
  string GenerateAccessToken(User user);
  RefreshToken GenerateRefreshToken(Guid userId);
}