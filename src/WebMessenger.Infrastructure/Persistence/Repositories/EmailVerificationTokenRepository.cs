using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Repositories;

namespace WebMessenger.Infrastructure.Persistence.Repositories;

public class EmailVerificationTokenRepository(WebMessengerDbContext dbContext) : IEmailVerificationTokenRepository
{
  public async Task<Guid> CreateAsync(EmailVerificationToken verificationToken)
  {
    verificationToken.Id = Guid.NewGuid();
    await dbContext.EmailVerificationTokens.AddAsync(verificationToken);
    await dbContext.SaveChangesAsync();
    return verificationToken.Id;
  }
  
  public async Task<EmailVerificationToken?> GetByTokenAsync(string token)
  {
    return await dbContext.EmailVerificationTokens.FirstOrDefaultAsync(t => t.Token == token);
  }

  public async Task<EmailVerificationToken?> GetByUserIdAsync(Guid userId)
  {
    return await dbContext.EmailVerificationTokens.FirstOrDefaultAsync(t => t.UserId == userId);
  }

  public async Task DeleteAsync(Guid id)
  {
    var verToken = await dbContext.EmailVerificationTokens.FindAsync(id);
    if (verToken == null)
      return;
    
    dbContext.EmailVerificationTokens.Remove(verToken);
    await dbContext.SaveChangesAsync();
  }
}