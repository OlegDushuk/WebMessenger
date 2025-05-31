using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Repositories;

public interface IUserTokenRepository
{
  Task<UserToken?> GetByTokenAsync(string token);
  Task<UserToken?> GetByUserIdAsync(Guid userId);
  Task<Guid> CreateAsync(UserToken verificationToken);
  Task DeleteAsync(Guid id);
}