using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Shared.Enums;

namespace WebMessenger.Web.Models;

public class ChatMemberModel(ChatMemberDto dto)
{
  public Guid Id { get; set; } = dto.Id;
  public bool IsMuted { get; set; } = dto.IsMuted;
  public ChatMemberRoleDto Role { get; set; } = dto.Role;
  
  public Guid UserId { get; set; } = dto.User.Id;
  public string UserName { get; set; } = dto.User.UserName;
  public string Name { get; set; } = dto.User.Name;
  public string? Avatar { get; set; } = dto.User.Avatar;
  
  public UserModel User { get; set; } = new(dto.User);
}