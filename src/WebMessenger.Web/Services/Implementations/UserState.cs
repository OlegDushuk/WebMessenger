using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class UserState : IUserState
{
  public UserModel? User { get; set; }
}