using WebMessenger.Shared.Enums;

namespace WebMessenger.Shared.DTOs.Responses;

public class GroupDetailsDto
{
  public string Name { get; set; } = null!;
  public string? Bio { get; set; }
  public string? Avatar { get; set; }
  public GroupTypeDto Type { get; set; }
}