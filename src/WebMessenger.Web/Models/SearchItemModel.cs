using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Shared.Enums;

namespace WebMessenger.Web.Models;

public class SearchItemModel(SearchItemDto dto)
{
  public Guid Id { get; set; } = dto.Id;
  public string Name { get; set; } = dto.Name;
  public string? UserName { get; set; } = dto.UserName;
  public string? Bio { get; set; } = dto.Bio;
  public string? Avatar { get; set; } = dto.Avatar;
  
  public SearchItemTypeDto Type { get; set; } = dto.Type;
}