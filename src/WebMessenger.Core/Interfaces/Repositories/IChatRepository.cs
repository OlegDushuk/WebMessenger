using WebMessenger.Core.Entities;

namespace WebMessenger.Core.Interfaces.Repositories;

public interface IChatRepository
{
  Task<bool> ExistsPrivateChatAsync(Guid user1, Guid user2);
  Task<Chat?> GetChatByIdAsync(Guid id);
  Task<List<Chat>> GetChatsByUserIdAsync(Guid userId);
  
  Task CreateChatAsync(Chat chat);
  Task DeleteChatAsync(Chat chat);
  
  Task<GroupDetails?> GetGroupDetailsByIdAsync(Guid id);
  
  Task CreateGroupDetailsAsync(GroupDetails groupChatDetails);
}