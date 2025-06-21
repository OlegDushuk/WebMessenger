using Microsoft.AspNetCore.Components;
using WebMessenger.Web.Models;

namespace WebMessenger.Web.Views.Shared.Chat;

public partial class ChatMemberView : ComponentBase
{
  [Parameter] public ChatMemberModel Member { get; set; } = null!;
  [Parameter] public EventCallback<ChatMemberModel> OnClickLink { get; set; }

  private async Task HandleMemberClick()
  {
    if (OnClickLink.HasDelegate)
      await OnClickLink.InvokeAsync(Member);
  }
}