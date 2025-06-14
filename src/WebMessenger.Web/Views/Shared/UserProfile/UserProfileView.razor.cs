using Microsoft.AspNetCore.Components;
using WebMessenger.Web.Models;

namespace WebMessenger.Web.Views.Shared.UserProfile;

public partial class UserProfileView : ComponentBase
{
  [Parameter] public UserModel Model { get; set; } = null!;
  [Parameter] public bool IsOwn { get; set; }
}