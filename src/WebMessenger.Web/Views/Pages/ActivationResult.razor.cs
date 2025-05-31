using Microsoft.AspNetCore.Components;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;

namespace WebMessenger.Web.Views.Pages;

public partial class ActivationResult : ComponentBase
{
  [Inject] private IAuthApi AuthApi { get; set; } = null!;

  private bool _isLoading = true;
  private bool _isSuccess;
  
  [Parameter] public string Token { get; set; } = string.Empty;
  
  protected override async Task OnInitializedAsync()
  {
    await HttpHelper.FetchAsync(async () => await AuthApi.VerifyAsync(Token),
      onSuccess: _ =>
      {
        _isSuccess = true;
        
        return Task.CompletedTask;
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));
        
        _isSuccess = false;
      },
      onException: _ =>
      {
        _isSuccess = false;
      }
    );
    
    _isLoading = false;
  }
}