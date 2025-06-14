using Blazored.LocalStorage;
using Microsoft.JSInterop;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class ThemeService(ILocalStorageService storage, IJSRuntime js) : IThemeService
{
  private const Theme DefaultTheme = Theme.Light;
  
  public async Task Initialize()
  {
    var theme = await storage.GetItemAsync<string>("Theme");
    if (theme == null)
    {
      await storage.SetItemAsync("Theme", DefaultTheme.ToString());
      await UpdateUi(DefaultTheme);
      return;
    }
    
    if (Enum.TryParse<Theme>(theme, true, out var result))
    {
      await UpdateUi(result);
    }
    else
    {
      await storage.SetItemAsync("Theme", DefaultTheme.ToString());
      await UpdateUi(DefaultTheme);
    }
  }

  public async Task SetTheme(Theme theme)
  {
    await storage.SetItemAsync("Theme", theme.ToString());
    await UpdateUi(theme);
  }
  
  public async Task<Theme> GetTheme()
  {
    var theme = await storage.GetItemAsync<string>("Theme");
    
    if (Enum.TryParse<Theme>(theme, true, out var result))
    {
      return result;
    }
    else
    {
      await storage.SetItemAsync("Theme", DefaultTheme.ToString());
      return DefaultTheme;
    }
  }
  
  private async Task UpdateUi(Theme theme)
  {
    await js.InvokeVoidAsync("setTheme", theme.ToString().ToLower());
  }
}