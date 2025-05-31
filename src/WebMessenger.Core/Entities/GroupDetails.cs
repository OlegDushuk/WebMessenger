using WebMessenger.Core.Enums;

namespace WebMessenger.Core.Entities;

public class GroupDetails
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? Bio { get; set; }
  public string? Avatar { get; set; }
  public GroupType Type { get; set; }
}