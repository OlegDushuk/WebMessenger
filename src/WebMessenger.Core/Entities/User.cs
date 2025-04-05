namespace WebMessenger.Core.Entities;

public class User
{
  public Guid Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public string? UserName { get; set; }
  public string? Email { get; set; }
  public string? PasswordHash { get; set; }
  public string? Name { get; set; }
  public string? Avatar { get; set; }
  public string? Bio { get; set; }
  public bool IsActive { get; set; }
}