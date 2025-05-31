using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMessenger.Core.Entities;

namespace WebMessenger.Infrastructure.Persistence.Configurations;

public class GroupDetailsConfiguration : IEntityTypeConfiguration<GroupDetails>
{
  public void Configure(EntityTypeBuilder<GroupDetails> builder)
  {
    builder.HasKey(c => c.Id);
    
    builder.HasIndex(x => x.Name);
    
    builder.Property(x => x.Name).HasMaxLength(32);
    builder.Property(x => x.Bio).HasMaxLength(128);
    builder.Property(x => x.Avatar).HasMaxLength(128);
    builder.Property(x => x.Type).HasConversion<int>().IsRequired();
    
    builder.HasOne<Chat>()
      .WithOne(x => x.GroupDetails)
      .HasForeignKey<GroupDetails>(x => x.Id)
      .OnDelete(DeleteBehavior.Cascade);
  }
}