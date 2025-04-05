using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMessenger.Core.Entities;

namespace WebMessenger.Infrastructure.Persistence.Configurations;

public class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
  public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
  {
    builder.HasKey(t => t.Id);
    
    builder.Property(t => t.Id).ValueGeneratedOnAdd();
    builder.Property(t => t.UserId).IsRequired();
    builder.Property(t => t.Token).IsRequired().HasMaxLength(128);
    builder.Property(t => t.ExpiresAt).IsRequired();
    
    builder.HasIndex(t => t.Token).IsUnique();
    builder.HasIndex(e => e.UserId);
    
    builder.HasOne<User>()
        .WithMany()
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}