using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Repositories;

public interface IChatMessageRepository
{
  Task<ChatMessage?> GetById(Guid id);
  Task<List<ChatMessage>> GetAllByChatId(Guid chatId);
  Task<List<ChatMessage>> GetAllByChatId(Guid chatId, int page, int pageSize);
  Task CreateAsync(ChatMessage message);
  Task UpdateAsync(ChatMessage message);
  Task DeleteAsync(ChatMessage message);
}