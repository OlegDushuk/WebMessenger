using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using WebMessenger.Application.Common.Helpers.Implementations;
using WebMessenger.Application.Common.Helpers.Interfaces;
using WebMessenger.Application.Common.Helpers.Settings;
using WebMessenger.Application.Mappings;
using WebMessenger.Application.UseCases.Implementations;
using WebMessenger.Application.UseCases.Interfaces;

namespace WebMessenger.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IChatService, ChatService>();
    services.AddScoped<ISearchService, SearchService>();
    
    services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
    
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddAutoMapper(typeof(MappingProfile));
    
    return services;
  }
}