using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebMessenger.Web;
using WebMessenger.Web.Services;
using WebMessenger.Web.Services.Implementations;
using WebMessenger.Web.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IAuthState, AuthState>();
builder.Services.AddScoped<IUserState, UserState>();
builder.Services.AddScoped<IChatState, ChatState>();
builder.Services.AddScoped<IThemeService, ThemeService>();

builder.Services.AddTransient(sp =>
{
  var authState = sp.GetRequiredService<IAuthState>();

  var httpClient = new HttpClient(new AuthHandler(authState))
  {
    BaseAddress = new Uri("https://app-25061900251.azurewebsites.net")
  };
  
  return httpClient;
});

builder.Services.AddTransient<IAuthApi, AuthApi>();
builder.Services.AddTransient<IAccountApi, AccountApi>();
builder.Services.AddTransient<IChatApi, ChatApi>();
builder.Services.AddTransient<ISearchApi, SearchApi>();

builder.Services.AddSingleton<IChatNotificationService, ChatNotificationService>();
builder.Services.AddSingleton<ChatViewState>();

await builder.Build().RunAsync();
