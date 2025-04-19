using System.ComponentModel.DataAnnotations;

namespace WebMessenger.Web.Models;

public class EmailModel
{
  [Required(ErrorMessage = "Це поле обов'язкове")]
  [EmailAddress(ErrorMessage = "Невірний формат пошти")]
  public string? Email { get; set; }
}