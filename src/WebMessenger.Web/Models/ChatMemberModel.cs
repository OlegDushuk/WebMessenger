using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Shared.Enums;

namespace WebMessenger.Web.Models;

public class ChatMemberModel(ChatMemberDto dto)
{
  public Guid Id { get; set; } = dto.Id;
  public bool IsMuted { get; set; } = dto.IsMuted;
  public ChatMemberRoleDto Role { get; set; } = dto.Role;

  public Guid UserId { get; set; } = dto.UserId;
  public string UserName { get; set; } = dto.UserName;
  public string Name { get; set; } = dto.Name;
  public string? Avatar { get; set; } = dto.Avatar;
}