using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Repositories;

namespace WebMessenger.Infrastructure.Persistence.Repositories;

public class UserTokenRepository(WebMessengerDbContext dbContext) : IUserTokenRepository
{
  public async Task<Guid> CreateAsync(UserToken verificationToken)
  {
    verificationToken.Id = Guid.NewGuid();
    await dbContext.UserTokens.AddAsync(verificationToken);
    await dbContext.SaveChangesAsync();
    return verificationToken.Id;
  }
  
  public async Task<UserToken?> GetByTokenAsync(string token)
  {
    return await dbContext.UserTokens.FirstOrDefaultAsync(t => t.Token == token);
  }

  public async Task<UserToken?> GetByUserIdAsync(Guid userId)
  {
    return await dbContext.UserTokens.FirstOrDefaultAsync(t => t.UserId == userId);
  }
  
  public async Task DeleteAsync(Guid id)
  {
    var verToken = await dbContext.UserTokens.FindAsync(id);
    if (verToken == null)
      return;
    
    dbContext.UserTokens.Remove(verToken);
    await dbContext.SaveChangesAsync();
  }
}