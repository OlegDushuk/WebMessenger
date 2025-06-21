using Microsoft.AspNetCore.Components;
using WebMessenger.Web.Models;

namespace WebMessenger.Web.Views.Shared.Chat;

public partial class EditChatForm : ComponentBase
{
  [Parameter] public CreateGroupModel Model { get; set; } = null!;
  [Parameter] public EventCallback OnValidSubmit { get; set; }
  
  private bool _isLoading;

  private async Task HandleValidSubmit()
  {
    _isLoading = true;
    
    if (OnValidSubmit.HasDelegate)
      await OnValidSubmit.InvokeAsync();
    
    _isLoading = false;
  }
}