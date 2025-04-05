using Microsoft.EntityFrameworkCore;
using WebMessenger.Core.Entities;
using WebMessenger.Infrastructure.Persistence.Configurations;

namespace WebMessenger.Infrastructure.Persistence;

public class WebMessengerDbContext(DbContextOptions<WebMessengerDbContext> options) : DbContext(options)
{
  public DbSet<User> Users { get; set; }
  public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
  public DbSet<RefreshToken> RefreshTokens { get; set; }
  public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new UserConfiguration());
    
    modelBuilder.ApplyConfiguration(new EmailVerificationTokenConfiguration());
    
    modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    
    modelBuilder.ApplyConfiguration(new ResetPasswordTokenConfiguration());

    base.OnModelCreating(modelBuilder);
  }
}