using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebMessenger.Core.Interfaces.Repositories;
using WebMessenger.Core.Interfaces.Services;
using WebMessenger.Infrastructure.ClientConnection;
using WebMessenger.Infrastructure.ExternalServices;
using WebMessenger.Infrastructure.Persistence;
using WebMessenger.Infrastructure.Persistence.Repositories;

namespace WebMessenger.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    // services.AddDbContext<WebMessengerDbContext>(options =>
    //   options.UseSqlServer(
    //     configuration.GetConnectionString("DefaultConnection"),
    //     sqlOptions =>
    //     {
    //       sqlOptions.MigrationsAssembly("WebMessenger.Infrastructure");
    //       sqlOptions.EnableRetryOnFailure();
    //     }
    //   ));

    services.AddDbContext<WebMessengerDbContext>(options =>
      options.UseSqlite(
        configuration.GetConnectionString("Sqlite"),
        x => x.MigrationsAssembly("WebMessenger.Infrastructure")
      ).LogTo(_ => { }));
    
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserTokenRepository, UserTokenRepository>();
    services.AddScoped<IChatRepository, ChatRepository>();
    services.AddScoped<IChatMemberRepository, ChatMemberRepository>();
    services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
    
    services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
    services.AddScoped<IEmailService, EmailService>();
    
    services.AddSingleton<ConnectionClientStorage>();
    services.AddScoped<IClientConnectionService, ClientConnectionService>();
    
    return services;
  }
}