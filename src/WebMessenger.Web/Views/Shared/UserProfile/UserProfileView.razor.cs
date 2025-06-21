using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Implementations;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;

namespace WebMessenger.Web.Views.Shared.UserProfile;

public partial class UserProfileView : ComponentBase
{
  [Parameter] public UserModel Model { get; set; } = null!;
  [Parameter] public bool IsOwn { get; set; }
  [Parameter] public EventCallback<UserModel> OnToChat { get; set; }

  private async Task HandleClick()
  {
    if (OnToChat.HasDelegate)
      await OnToChat.InvokeAsync(Model);
  }
}