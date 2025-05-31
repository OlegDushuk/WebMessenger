using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMessenger.Core.Entities;

namespace WebMessenger.Infrastructure.Persistence.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
  public void Configure(EntityTypeBuilder<UserToken> builder)
  {
    builder.HasKey(t => t.Id);
    
    builder.Property(ut => ut.Id).ValueGeneratedOnAdd();
    builder.Property(ut => ut.UserId).IsRequired();
    builder.Property(ut => ut.Token).IsRequired().HasMaxLength(128);
    builder.Property(ut => ut.ExpiresAt).IsRequired();
    
    builder.HasIndex(ut => ut.Token).IsUnique();
    builder.HasIndex(ut => ut.UserId);
    
    builder.HasOne<User>()
      .WithMany()
      .HasForeignKey(ut => ut.UserId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}