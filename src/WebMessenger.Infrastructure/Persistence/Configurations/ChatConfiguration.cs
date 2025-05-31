using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMessenger.Core.Entities;

namespace WebMessenger.Infrastructure.Persistence.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
  public void Configure(EntityTypeBuilder<Chat> builder)
  {
    builder.HasKey(c => c.Id);
    
    builder.Property(c => c.Id).ValueGeneratedOnAdd();
    builder.Property(c => c.CreatedAt).IsRequired();
  }
}