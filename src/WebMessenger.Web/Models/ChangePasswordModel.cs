using System.ComponentModel.DataAnnotations;

namespace WebMessenger.Web.Models;

public class ChangePasswordModel
{
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [DataType(DataType.Password)]
  public string OldPassword { get; set; } = string.Empty;
  
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [DataType(DataType.Password)]
  [MinLength(6, ErrorMessage = "Має містити мінімум 6 символів")]
  public string NewPassword { get; set; } = string.Empty;
  
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [DataType(DataType.Password)]
  [Compare("NewPassword", ErrorMessage = "Паролі не збігаються")]
  public string ConfirmNewPassword { get; set; } = string.Empty;
}