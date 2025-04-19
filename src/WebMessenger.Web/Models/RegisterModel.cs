using System.ComponentModel.DataAnnotations;

namespace WebMessenger.Web.Models;

public class RegisterModel
{
  public string? Name { get; set; }
  
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [MinLength(3, ErrorMessage = "Має містити мінімум 3 символа")]
  public string? UserName { get; set; }
  
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [EmailAddress(ErrorMessage = "Невірний формат пошти")]
  public string? Email { get; set; }
  
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [DataType(DataType.Password)]
  [MinLength(6, ErrorMessage = "Має містити мінімум 6 символів")]
  public string? Password { get; set; }
  
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "Паролі не збігаються")]
  public string? ConfirmPassword { get; set; }
}