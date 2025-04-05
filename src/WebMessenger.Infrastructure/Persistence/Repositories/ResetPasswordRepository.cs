using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Repositories;

namespace WebMessenger.Infrastructure.Persistence.Repositories;

public class ResetPasswordRepository(WebMessengerDbContext dbContext) : IResetPasswordTokenRepository
{
  public async Task<ResetPasswordToken?> GetByTokenAsync(string dtoToken)
  {
    return await dbContext.ResetPasswordTokens.FirstOrDefaultAsync(rpt => rpt.Token == dtoToken);
  }

  public async Task<Guid> CreateAsync(ResetPasswordToken token)
  {
    token.Id = Guid.NewGuid();
    await dbContext.ResetPasswordTokens.AddAsync(token);
    await dbContext.SaveChangesAsync();
    return token.Id;
  }

  public async Task DeleteAsync(ResetPasswordToken token)
  {
    dbContext.ResetPasswordTokens.Remove(token);
    await dbContext.SaveChangesAsync();
  }
}