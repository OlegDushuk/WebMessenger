using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Infrastructure.Persistence.Configurations;

namespace WebMessenger.Infrastructure.Persistence;

public class WebMessengerDbContext(DbContextOptions<WebMessengerDbContext> options) : DbContext(options)
{
  public DbSet<User> Users { get; set; }
  public DbSet<UserToken> UserTokens { get; set; }
  public DbSet<Chat> Chats { get; set; }
  public DbSet<GroupDetails> GroupDetails { get; set; }
  public DbSet<ChatMember> ChatMembers { get; set; }
  public DbSet<ChatMessage> ChatsMessages { get; set; }
  public DbSet<UnreadMessage> UnreadMessages { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new UserConfiguration());
    modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
    modelBuilder.ApplyConfiguration(new ChatConfiguration());
    modelBuilder.ApplyConfiguration(new GroupDetailsConfiguration());
    modelBuilder.ApplyConfiguration(new ChatMemberConfiguration());
    modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
    modelBuilder.ApplyConfiguration(new UnreadMessageConfiguration());

    base.OnModelCreating(modelBuilder);
  }
}