using System.ComponentModel.DataAnnotations;

namespace WebMessenger.Web.Models;

public class ResetPasswordModel
{
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [DataType(DataType.Password)]
  [MinLength(6, ErrorMessage = "Має містити мінімум 6 символів")]
  public string? NewPassword { get; set; }
  
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [DataType(DataType.Password)]
  [Compare("NewPassword", ErrorMessage = "Паролі не збігаються")]
  public string? ConfirmPassword { get; set; }
}