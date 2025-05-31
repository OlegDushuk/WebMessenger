using Microsoft.AspNetCore.Components;
using WebMessenger.Web.Models;

namespace WebMessenger.Web.Views.Shared.Chat;

public partial class MessageView : ComponentBase
{
  [Parameter] public MessageModel Message { get; set; } = null!;
  
  private bool IsOwn => Message.IsOwn;
}