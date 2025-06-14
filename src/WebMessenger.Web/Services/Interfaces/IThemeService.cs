namespace WebMessenger.Web.Services.Interfaces;

public enum Theme
{
  Light = 0,
  Dark = 1
}

public interface IThemeService
{
  Task Initialize();
  Task SetTheme(Theme theme);
  Task<Theme> GetTheme();
}