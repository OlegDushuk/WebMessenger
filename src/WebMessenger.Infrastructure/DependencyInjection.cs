using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMessenger.Core.Interfaces.Repositories;
using WebMessenger.Core.Interfaces.Services;
using WebMessenger.Infrastructure.ClientConnection;
using WebMessenger.Infrastructure.ExternalServices;
using WebMessenger.Infrastructure.Persistence;
using WebMessenger.Infrastructure.Persistence.Common.Enums;
using WebMessenger.Infrastructure.Persistence.Repositories;

namespace WebMessenger.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    var value = configuration.GetValue<string>("DbType");
    if (!Enum.TryParse<DbType>(value, true, out var dbType))
      throw new InvalidOperationException($"Db type: {value} is undefined.");
    
    switch (dbType)
    {
      case DbType.SqLite:
        services.AddDbContext<WebMessengerDbContext>(options =>
          options.UseSqlite(
              configuration.GetConnectionString("Sqlite"),
              x => x.MigrationsAssembly(typeof(WebMessengerDbContext).Assembly.FullName))
            .LogTo(_ => { }));
        break;
      
      case DbType.SqlServer:
        services.AddDbContext<WebMessengerDbContext>(options =>
          options.UseSqlServer(
              configuration.GetConnectionString("SqlServer"),
              sqlOptions =>
              {
                sqlOptions.MigrationsAssembly(typeof(WebMessengerDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure();
              })
            .LogTo(_ => { }));
        break;
      
      default:
        throw new InvalidOperationException($"Db type: {dbType} is not supported.");
    }

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