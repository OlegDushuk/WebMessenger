using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared.Modal;

namespace WebMessenger.Web.Views.Shared.Chat;

public partial class CreateChatForm : ComponentBase
{
  [CascadingParameter(Name = "GlobalModal")] public ModalView GlobalModal { get; set; } = null!;
  
  [Inject] private IChatState ChatState { get; set; } = null!;
  [Inject] private IChatNotificationService ChatNotificationService { get; set; } = null!;
  [Inject] private IChatApi ChatApi { get; set; } = null!;
  
  private CreateGroupModel _model = new();
  
  private async Task HandleCreateChat()
  {
    await HttpHelper.FetchAsync(() => ChatApi.CreateGroupAsync(new CreateGroupChatDto
    {
      Name = _model.Name!,
      Bio = _model.Bio!,
      Avatar = _model.AvatarUrl,
      Type = _model.Type,
    }),
      onSuccess: async response =>
      {
        var chatDto = await response.Content.ReadFromJsonAsync<ChatDto>();
        if (chatDto == null)
          throw new NullReferenceException(nameof(chatDto));
        
        ChatState.AddChat(new ChatModel(chatDto));
        await ChatNotificationService.ConnectToChatAsync(chatDto.Id);
        
        _model = new CreateGroupModel();
        GlobalModal.Close();
      },
      onFailure: async response =>
      {
        var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        if (errors == null)
          throw new NullReferenceException(nameof(errors));
        
        
      },
      onException: exception =>
      {
        
      });
  }
}