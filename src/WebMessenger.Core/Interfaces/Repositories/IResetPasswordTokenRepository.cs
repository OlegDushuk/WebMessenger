using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Repositories;

public interface IResetPasswordTokenRepository
{
  Task<ResetPasswordToken?> GetByTokenAsync(string dtoToken);
  Task<Guid> CreateAsync(ResetPasswordToken token);
  Task DeleteAsync(ResetPasswordToken token);
}