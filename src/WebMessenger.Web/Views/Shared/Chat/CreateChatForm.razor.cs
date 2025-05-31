using Microsoft.AspNetCore.Components;
using WebMessenger.Core.Enums;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Views.Shared.Modal;

namespace WebMessenger.Web.Views.Shared.Chat;

public partial class CreateChatForm : ComponentBase
{
  [CascadingParameter(Name = "GlobalModal")] public ModalView GlobalModal { get; set; } = null!;
  
  [Inject] private IChatState ChatState { get; set; } = null!;
  
  private CreateGroupModel _model = new();
  
  private async Task HandleCreateChat()
  {
    
    
    _model = new CreateGroupModel();
    GlobalModal.Close();
  }
}