using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using WebMessenger.Application.UseCases.Implementations;
using WebMessenger.Application.UseCases.Interfaces;

namespace WebMessenger.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IAccountService, AccountService>();
    
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    
    return services;
  }
}