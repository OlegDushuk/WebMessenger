using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Repositories;

public interface IEmailVerificationTokenRepository
{
  Task<Guid> CreateAsync(EmailVerificationToken verificationToken);
  Task<EmailVerificationToken?> GetByTokenAsync(string token);
  Task<EmailVerificationToken?> GetByUserIdAsync(Guid userId);
  Task DeleteAsync(Guid id);
}