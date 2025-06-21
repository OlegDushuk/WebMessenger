using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebMessenger.Shared.Enums;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Views.Shared.Chat;

public partial class ChatManagementModal : ComponentBase
{
  [Parameter] public ChatModel Chat { get; set; } = null!;
  
  [Inject] private IUserState UserState { get; set; } = null!;
  
  private CreateGroupModel _editChatModel = null!;

  private string _currentView = "info";
  private ChatMemberModel _selectedMember= null!;
  
  private async Task AddNewMember()
  {
    
  }
  
  private async Task ToEdit()
  {
    _currentView = "edit";
    await InvokeAsync(StateHasChanged);
    
    _editChatModel = new CreateGroupModel
    {
      Name = Chat.Name,
      Bio = Chat.Bio,
      AvatarUrl = Chat.AvatarUrl,
      Type = (GroupTypeDto)Chat.GroupType!
    };
  }
  
  private async Task ToInfo()
  {
    _currentView = "info";
    await InvokeAsync(StateHasChanged);
  }
  
  private async Task ToAddMember()
  {
    _currentView = "addMember";
    await InvokeAsync(StateHasChanged);
  }

  private async Task ToMember(ChatMemberModel member)
  {
    _selectedMember = member;
    _currentView = "member";
    await InvokeAsync(StateHasChanged);
  }
}