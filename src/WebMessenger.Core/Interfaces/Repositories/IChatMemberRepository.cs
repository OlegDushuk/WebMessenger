using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Repositories;

public interface IChatMemberRepository
{
  Task<ChatMember?> GetMemberByIdAsync(Guid id);
  Task<ChatMember?> GetMemberAsync(Guid chatId, Guid userId);
  Task<List<ChatMember>> GetMembersByUserId(Guid userId);
  Task<List<ChatMember>> GetMembersByChatId(Guid chatId);
  Task<ChatMember?> GetOtherPrivateChatMemberByChatId(Guid chatId, Guid currentUserId);
  Task AddMemberAsync(ChatMember chatMember);
  Task UpdateMemberAsync(ChatMember chatMember);
  Task DeleteMemberAsync(ChatMember chatMember);
}