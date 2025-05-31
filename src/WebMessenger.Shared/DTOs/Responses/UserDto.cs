namespace WebMessenger.Shared.DTOs.Responses;

public class UserDto
{
  public Guid Id { get; set; }
  public string UserName { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? Avatar { get; set; }
  public string? Bio { get; set; }
}