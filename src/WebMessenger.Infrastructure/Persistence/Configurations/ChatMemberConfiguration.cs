using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMessenger.Core.Entities;

namespace WebMessenger.Infrastructure.Persistence.Configurations;

public class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
{
  public void Configure(EntityTypeBuilder<ChatMember> builder)
  {
    builder.HasKey(x => x.Id);
    
    builder.HasIndex(x => new { x.UserId, x.ChatId }).IsUnique();
    builder.HasIndex(x => x.UserId);
    builder.HasIndex(x => x.ChatId);
    
    builder.Property(x => x.Id).ValueGeneratedOnAdd();
    builder.Property(x => x.Role).HasConversion<int>().IsRequired();
    
    builder.HasOne(m => m.User)
      .WithMany()
      .HasForeignKey(x => x.UserId)
      .OnDelete(DeleteBehavior.NoAction);
    
    builder.HasOne(m => m.Chat)
      .WithMany()
      .HasForeignKey(x => x.ChatId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}