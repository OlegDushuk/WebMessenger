using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared.Modal;

namespace WebMessenger.Web.Views.Shared.Chat;

public partial class ChatView : ComponentBase
{
  private ChatModel? Model => ChatState.CurrentChat;
  
  [Inject] public IJSRuntime Js { get; set; } = null!;
  [Inject] public IChatState ChatState { get; set; } = null!;
  [Inject] public IChatApi ChatApi { get; set; } = null!;
  
  private ModalTemplate _chatInfoModal = null!;
  
  private bool _isLoadingHistory;
  private bool _isSendingMessage;

  protected override async Task OnInitializedAsync()
  {
    ChatState.OnChatSelected += async () =>
    {
      if (Model == null) return;
      
      Model.OnChangeState += () => { InvokeAsync(StateHasChanged); };
      await ScrollToBottom();
        
      if (Model.FirstSelected)
      {
        await LoadMembers();
        await LoadNextPage();
      
        Model.FirstSelected = false;
      }
      
      await InvokeAsync(StateHasChanged);
    };
    
    ChatState.OnChatExit += () =>
    {
      Model!.OnChangeState -= () => { InvokeAsync(StateHasChanged); };
    };
    
    await base.OnInitializedAsync();
  }
  
  private async Task SendMessage()
  {
    if (string.IsNullOrEmpty(Model!.CurrentMessage))
      return;
    
    _isSendingMessage = true;
    
    await HttpHelper.FetchAsync(() => ChatApi.SendMessageAsync(new SendMessageDto
      {
        Content = Model.CurrentMessage,
        MemberId = Model.CurrentMember.Id
      }),
      onSuccess: async response =>
      {
        var messageDto = await response.Content.ReadFromJsonAsync<ChatMessageDto>();
        if (messageDto == null)
          throw new NullReferenceException(nameof(messageDto));
        
        Console.WriteLine("Member: " + Model.CurrentMember.Id);
        Console.WriteLine("Chat: " + Model.Id);
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));
        
        
      },
      onException: exception =>
      {
        
      });
    
    _isSendingMessage = false;
    Model.CurrentMessage = string.Empty;
  }
  
  private async Task OnScroll()
  {
    var scrollPercentage = await Js.InvokeAsync<double>("getScrollPercentage", "chatContainer");
    if (scrollPercentage < 20)
    {
      await LoadNextPage();
    }
  }
  
  private async Task LoadNextPage()
  {
    if (Model!.AllMessagesLoaded || _isLoadingHistory)
      return;
    
    var prevHeight = await GetScrollHeight();

    _isLoadingHistory = true;
    
    await HttpHelper.FetchAsync(async () =>
        await ChatApi.GetChatHistoryAsync(Model.Id, Model.MessagePage, 20),
      onSuccess: async response =>
      {
        var history = await response.Content.ReadFromJsonAsync<List<ChatMessageDto>>();
        if (history == null)
          throw new NullReferenceException(nameof(history));
        
        if (history.Count != 0)
        {
          var messages = history.Select(message =>
          {
            var sender = Model.Members.First(m => message.MemberId == m.Id);
            var model = new MessageModel(message, sender, sender.UserId == Model.CurrentMember.UserId);
            return model;
          });
          
          Model.AddHistory(messages);
          Model.MessagePage++;

          await SaveScroll(prevHeight);
        }
        else
        {
          Model.AllMessagesLoaded = true;
        }
      },
      onFailure: async response =>
      {
        
      });
    
    _isLoadingHistory = false;
  }

  private async Task LoadMembers()
  {
    await HttpHelper.FetchAsync(async () => await ChatApi.GetChatMembers(Model!.Id),
      onSuccess: async response =>
      {
        var members = await response.Content.ReadFromJsonAsync<List<ChatMemberDto>>();
        if (members == null)
          throw new NullReferenceException(nameof(members));
        
        var memberModels = members.Select(member =>
          new ChatMemberModel(member));
        Model!.AddMembers(memberModels);
      },
      onFailure: async response =>
      {
        
      });
  }
  
  private async Task ScrollToBottom()
  {
    await Js.InvokeVoidAsync("scrollToBottom");
  }

  private async Task<double> GetScrollHeight()
  {
    return await Js.InvokeAsync<double>("getScrollHeight");
  }
  
  private async Task SaveScroll(double prevHeight)
  {
    await Js.InvokeAsync<double>("updateScroll", prevHeight);
  }
}