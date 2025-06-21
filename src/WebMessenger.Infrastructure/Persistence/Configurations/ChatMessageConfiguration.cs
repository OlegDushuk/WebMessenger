using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMessenger.Core.Entities;

namespace WebMessenger.Infrastructure.Persistence.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
  public void Configure(EntityTypeBuilder<ChatMessage> builder)
  {
    builder.HasKey(x => x.Id);
    
    builder.HasIndex(x => x.ChatId);
    
    builder.Property(x => x.Id).ValueGeneratedOnAdd();
    builder.Property(x => x.SendAt).IsRequired();
    builder.Property(x => x.Content).HasMaxLength(512).IsRequired();
    
    builder.HasOne<Chat>()
      .WithMany()
      .HasForeignKey(x => x.ChatId)
      .OnDelete(DeleteBehavior.Cascade);
    
    builder.HasOne<ChatMember>()
      .WithMany()
      .HasForeignKey(x => x.MemberId)
      .OnDelete(DeleteBehavior.NoAction);
  }
}