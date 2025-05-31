using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Repositories;

namespace WebMessenger.Infrastructure.Persistence.Repositories;

public class ChatMemberRepository(WebMessengerDbContext dbContext) : IChatMemberRepository
{
  public async Task<ChatMember?> GetMemberByIdAsync(Guid id)
  {
    return await dbContext.ChatMembers
      .Include(cm => cm.User)
      .Include(cm => cm.Chat)
      .FirstOrDefaultAsync(cm => cm.Id == id);
  }

  public async Task<ChatMember?> GetMemberAsync(Guid chatId, Guid userId)
  {
    return await dbContext.ChatMembers
      .Include(cm => cm.User)
      .Include(cm => cm.Chat)
      .FirstOrDefaultAsync(cm => cm.ChatId == chatId && cm.UserId == userId);
  }
  
  public async Task<List<ChatMember>> GetMembersByUserId(Guid userId)
  {
    return await dbContext.ChatMembers
      .Include(cm => cm.Chat)
      .ThenInclude(c => c.GroupDetails)
      .Where(cm => cm.UserId == userId)
      .ToListAsync();
  }

  public async Task<List<ChatMember>> GetMembersByChatId(Guid chatId)
  {
    return await dbContext.ChatMembers
      .Include(cm => cm.User)
      .Where(cm => cm.ChatId == chatId)
      .ToListAsync();
  }
  
  public async Task<ChatMember?> GetOtherPrivateChatMemberByChatId(Guid chatId, Guid currentUserId)
  {
    return await dbContext.ChatMembers
      .Include(cm => cm.User)
      .Where(cm => cm.ChatId == chatId && cm.UserId != currentUserId)
      .FirstOrDefaultAsync();
  }

  public async Task AddMemberAsync(ChatMember chatMember)
  {
    dbContext.ChatMembers.Add(chatMember);
    await dbContext.SaveChangesAsync();
  }
  
  public async Task UpdateMemberAsync(ChatMember chatMember)
  {
    dbContext.ChatMembers.Update(chatMember);
    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteMemberAsync(ChatMember chatMember)
  {
    dbContext.ChatMembers.Remove(chatMember);
    await dbContext.SaveChangesAsync();
  }
}