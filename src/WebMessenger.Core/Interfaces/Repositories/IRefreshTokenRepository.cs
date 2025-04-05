using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
  Task<RefreshToken?> GetByTokenAsync(string token);
  Task<Guid> CreateAsync(RefreshToken refreshToken);
  Task DeleteAsync(Guid id);
}