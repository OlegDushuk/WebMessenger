using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMessenger.Core.Interfaces.Repositories;
using WebMessenger.Core.Interfaces.Services;
using WebMessenger.Infrastructure.ExternalServices;
using WebMessenger.Infrastructure.Persistence;
using WebMessenger.Infrastructure.Persistence.Repositories;

namespace WebMessenger.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<WebMessengerDbContext>(options =>
      options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("WebMessenger.Infrastructure")));
    
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
    services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    services.AddScoped<IResetPasswordTokenRepository, ResetPasswordRepository>();
    
    services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
    services.AddScoped<IEmailService, EmailService>();

    services.AddScoped<IPasswordService, PasswordService>();
    
    services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    services.AddScoped<IAuthTokenService, AuthTokenService>();
    
    return services;
  }
}