using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Enums;
using WebMessenger.Core.Interfaces.Repositories;

namespace WebMessenger.Infrastructure.Persistence.Repositories;

public class ChatRepository(WebMessengerDbContext dbContext) : IChatRepository
{
  public async Task<bool> ExistsPrivateChatAsync(Guid user1, Guid user2)
  {
    return await dbContext.Chats
      .Where(c => c.Type == ChatType.Personal)
      .Where(c => dbContext.ChatMembers
        .Where(gm => gm.ChatId == c.Id)
        .Select(gm => gm.UserId)
        .Distinct()
        .Contains(user1))
      .Where(c => dbContext.ChatMembers
        .Where(gm => gm.ChatId == c.Id)
        .Select(gm => gm.UserId)
        .Distinct()
        .Contains(user2))
      .AnyAsync();
  }

  public async Task<Chat?> GetChatByIdAsync(Guid id)
  {
    return await dbContext.Chats
      .Include(c => c.GroupDetails)
      .FirstOrDefaultAsync(c => c.Id == id);
  }
  
  public async Task<List<Chat>> GetChatsByUserIdAsync(Guid userId)
  {
    return await dbContext.Chats
      .Include(c => c.GroupDetails)
      .Where(chat => dbContext.ChatMembers
        .Any(cm => cm.ChatId == chat.Id && cm.UserId == userId))
      .ToListAsync();
  }
  
  public async Task CreateChatAsync(Chat chat)
  {
    dbContext.Chats.Add(chat);
    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteChatAsync(Chat chat)
  {
    dbContext.Chats.Remove(chat);
    await dbContext.SaveChangesAsync();
  }

  public async Task<GroupDetails?> GetGroupDetailsByIdAsync(Guid id)
  {
    return await dbContext.GroupDetails.FindAsync(id);
  }

  public async Task CreateGroupDetailsAsync(GroupDetails groupChatDetails)
  {
    dbContext.GroupDetails.Add(groupChatDetails);
    await dbContext.SaveChangesAsync();
  }
}