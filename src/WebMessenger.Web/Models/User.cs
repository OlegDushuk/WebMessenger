namespace WebMessenger.Web.Models;

public class User
{
  public string Email { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public string? Name { get; set; }
  public string? AvatarUrl { get; set; }
  public string? Bio { get; set; }
}