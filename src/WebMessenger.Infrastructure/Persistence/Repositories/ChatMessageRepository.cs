using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Repositories;

namespace WebMessenger.Infrastructure.Persistence.Repositories;

public class ChatMessageRepository(WebMessengerDbContext dbContext) : IChatMessageRepository
{
  public async Task<ChatMessage?> GetById(Guid id)
  {
    return await dbContext.ChatsMessages.FindAsync(id);
  }

  public async Task<List<ChatMessage>> GetAllByChatId(Guid chatId)
  {
    return await dbContext.ChatsMessages.Where(cm => cm.ChatId == chatId).ToListAsync();
  }

  public async Task<List<ChatMessage>> GetAllByChatId(Guid chatId, int page, int pageSize)
  {
    return await dbContext.ChatsMessages
      .Where(m => m.ChatId == chatId)
      .OrderByDescending(m => m.SendAt)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .OrderBy(m => m.SendAt)
      .ToListAsync();
  }

  public async Task CreateAsync(ChatMessage message)
  {
    dbContext.ChatsMessages.Add(message);
    await dbContext.SaveChangesAsync();
  }

  public async Task UpdateAsync(ChatMessage message)
  {
    dbContext.ChatsMessages.Update(message);
    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteAsync(ChatMessage message)
  {
    dbContext.ChatsMessages.Remove(message);
    await dbContext.SaveChangesAsync();
  }
}