using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebMessenger.Web;
using WebMessenger.Web.Services.Implementations;
using WebMessenger.Web.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAuthState, AuthState>();
builder.Services.AddScoped<IUserState, UserState>();

builder.Services.AddTransient(sp =>
{
  var authState = sp.GetRequiredService<IAuthState>();

  var httpClient = new HttpClient(new AuthHandler(authState))
  {
    BaseAddress = new Uri("https://localhost:7276/")
  };
  
  return httpClient;
});

builder.Services.AddTransient<IAuthApiSource, AuthApiSource>();
builder.Services.AddTransient<IAccountApiSource, AccountApiSource>();

await builder.Build().RunAsync();
