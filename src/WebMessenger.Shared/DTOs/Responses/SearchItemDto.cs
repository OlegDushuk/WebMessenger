using WebMessenger.Shared.Enums;

namespace WebMessenger.Shared.DTOs.Responses;

public class SearchItemDto
{
  public Guid ItemId { get; set; }
  public string Name { get; set; } = null!;
  public string? UserName { get; set; }
  public string? Avatar { get; set; }
  
  public SearchItemTypeDto Type { get; set; }
}