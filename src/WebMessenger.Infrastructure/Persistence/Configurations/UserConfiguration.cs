using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebMessenger.Core.Entities;

namespace WebMessenger.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasKey(u => u.Id);
    
    builder.Property(u => u.Id).ValueGeneratedNever();
    builder.Property(u => u.CreatedAt).IsRequired();
    builder.Property(u => u.UserName).IsRequired().HasMaxLength(63);
    builder.Property(u => u.Email).IsRequired().HasMaxLength(127);
    builder.Property(u => u.PasswordHash).IsRequired();
    builder.Property(u => u.Name).HasMaxLength(127);
    builder.Property(u => u.Avatar).HasMaxLength(127);
    builder.Property(u => u.Bio).HasMaxLength(255);
    
    builder.HasIndex(u => u.Email).IsUnique();
    builder.HasIndex(u => u.UserName).IsUnique();
  }
}