using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Repositories;

namespace WebMessenger.Infrastructure.Persistence.Repositories;

public class UserRepository(WebMessengerDbContext dbContext) : IUserRepository
{
  public async Task<bool> ExistsByEmailAsync(string email)
  {
    return await dbContext.Users.AnyAsync(u => u.Email == email);
  }

  public async Task<bool> ExistsByUserNameAsync(string userName)
  {
    return await dbContext.Users.AnyAsync(u => u.UserName == userName);
  }

  public async Task<User?> GetByIdAsync(Guid id)
  {
    return await dbContext.Users.FindAsync(id);
  }

  public async Task<User?> GetByEmailAsync(string email)
  {
    return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task<User?> GetByUserNameAsync(string userName)
  {
    return await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
  }

  public async Task<Guid> CreateAsync(User user)
  {
    user.Id = Guid.NewGuid();
    user.CreatedAt = DateTime.UtcNow;
    await dbContext.Users.AddAsync(user);
    await dbContext.SaveChangesAsync();
    return user.Id;
  }

  public async Task UpdateAsync(User user)
  {
    dbContext.Users.Update(user);
    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteAsync(User user)
  {
    dbContext.Users.Remove(user);
    await dbContext.SaveChangesAsync();
  }
}