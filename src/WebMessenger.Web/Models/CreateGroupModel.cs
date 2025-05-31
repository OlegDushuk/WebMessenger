using System.ComponentModel.DataAnnotations;
using WebMessenger.Core.Enums;

namespace WebMessenger.Web.Models;

public class CreateGroupModel
{
  [Required(ErrorMessage = "Це поле обов'язкове")]
  public string? Name { get; set; }
  public string? Bio { get; set; }
  public string? AvatarUrl { get; set; }
  public GroupType Type { get; set; }
}