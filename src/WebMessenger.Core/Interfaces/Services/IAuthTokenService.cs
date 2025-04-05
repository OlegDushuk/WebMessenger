using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Services;

public interface IAuthTokenService
{
  (string, DateTime) GenerateAccessToken(User user);
  RefreshToken GenerateRefreshToken(Guid userId);
}