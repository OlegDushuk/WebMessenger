using WebMessenger.Web.Models;

namespace WebMessenger.Web.Services.Interfaces;

public interface IUserState
{
  UserModel? User { get; set; }
}