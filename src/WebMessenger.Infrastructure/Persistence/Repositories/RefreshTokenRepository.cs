using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Repositories;

namespace WebMessenger.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(WebMessengerDbContext dbContext) : IRefreshTokenRepository
{
  public async Task<RefreshToken?> GetByTokenAsync(string token)
  {
    return await dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
  }

  public async Task<Guid> CreateAsync(RefreshToken refreshToken)
  {
    refreshToken.Id = Guid.NewGuid();
    await dbContext.RefreshTokens.AddAsync(refreshToken);
    await dbContext.SaveChangesAsync();
    return refreshToken.Id;
  }
  
  public async Task DeleteAsync(Guid id)
  {
    var refreshToken = await dbContext.RefreshTokens.FindAsync(id);
    dbContext.RefreshTokens.Remove(refreshToken!);
    await dbContext.SaveChangesAsync();
  }
}