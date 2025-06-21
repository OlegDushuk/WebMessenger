using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Shared.Enums;

namespace WebMessenger.Web.Models;

public class SearchItemModel(SearchItemDto dto)
{
  public string? Avatar { get; set; } = dto.Type == SearchItemTypeDto.User ?
    dto.User!.Avatar : dto.Chat!.GroupDetails!.Avatar;
  
  public string Name { get; set; } = dto.Type == SearchItemTypeDto.User ?
    dto.User!.Name : dto.Chat!.GroupDetails!.Name;
  
  public UserModel? User { get; set; } = dto.User != null ? new UserModel(dto.User) : null;
  public ChatModel? Chat { get; set; } = dto.Chat != null ? new ChatModel(dto.Chat) : null;
  
  public SearchItemTypeDto Type { get; set; } = dto.Type;
}