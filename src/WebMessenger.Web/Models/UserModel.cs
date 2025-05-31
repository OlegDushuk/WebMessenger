using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Web.Models;

public class UserModel(UserDto dto)
{
  public Guid Id { get; set; } = dto.Id;
  public string Email { get; set; } = dto.Email;
  public string UserName { get; set; } = dto.UserName;
  public string? Name { get; set; } = dto.Name;
  public string? AvatarUrl { get; set; } = dto.Avatar;
  public string? Bio { get; set; } = dto.Bio;
}