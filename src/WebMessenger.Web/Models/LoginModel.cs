using System.ComponentModel.DataAnnotations;

namespace WebMessenger.Web.Models;

public class LoginModel
{
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [EmailAddress(ErrorMessage = "Невірний формат пошти")]
  public string? Email { get; set; }
  
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [DataType(DataType.Password)]
  [MinLength(6, ErrorMessage = "Має містити мінімум 6 символів")]
  public string? Password { get; set; }
  
  public bool RememberMe { get; set; }
}