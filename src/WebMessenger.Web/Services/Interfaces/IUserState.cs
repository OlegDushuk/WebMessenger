using WebMessenger.Web.Models;

namespace WebMessenger.Web.Services.Interfaces;

public interface IUserState
{
  User? User { get; set; }
}