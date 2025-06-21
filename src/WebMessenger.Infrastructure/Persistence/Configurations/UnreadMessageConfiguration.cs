using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMessenger.Core.Entities;

namespace WebMessenger.Infrastructure.Persistence.Configurations;

public class UnreadMessageConfiguration : IEntityTypeConfiguration<UnreadMessage>
{
  public void Configure(EntityTypeBuilder<UnreadMessage> builder)
  {
    builder.HasKey(x => new { x.MemberId, x.MessageId });
    
    builder.HasOne<ChatMember>()
      .WithMany()
      .HasForeignKey(x => x.MemberId)
      .OnDelete(DeleteBehavior.NoAction);
    
    builder.HasOne<ChatMessage>()
      .WithMany()
      .HasForeignKey(x => x.MessageId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}