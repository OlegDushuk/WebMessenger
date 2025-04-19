using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Repositories;

public interface IUserRepository
{
  Task<bool> ExistsByEmailAsync(string email);
  Task<bool> ExistsByUserNameAsync(string userName);
  Task<User?> GetByIdAsync(Guid id);
  Task<User?> GetByEmailAsync(string email);
  Task<Guid> CreateAsync(User user);
  Task UpdateAsync(User user);
  Task DeleteAsync(User user);
}