using Microsoft.AspNetCore.Components;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared.Auth;

namespace WebMessenger.Web.Views.Pages;

public partial class ActivationAccount : ComponentBase
{
  [SupplyParameterFromQuery] public string? Email { get; set; }
  
  [Inject] private IAuthApi AuthApi { get; set; } = null!;

  private readonly EmailModel _model = new();
  private bool _isLoading;
  private bool _isSuccess;
  private string? _error;
  
  protected override async Task OnInitializedAsync()
  {
    _model.Email = Email;
    
    await base.OnInitializedAsync();
  }
  
  private async Task SendEmail(AuthForm<EmailModel>.SubmitCallbackArgs args)
  {
    _isLoading = true;
    _isSuccess = false;
    _error = null;
    
    await HttpHelper.FetchAsync(async () => await AuthApi.SendVerificationLinkAsync(_model.Email ?? ""),
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
        
        var displayError = ErrorToDisplayMessageMapper.ToDisplayMessage(error);
        _error = displayError;
      },
      onException: exception =>
      {
        var displayError = ErrorToDisplayMessageMapper.ToDisplayMessage(exception.Message);
        _error = displayError;
      }
    );
    
    _isLoading = false;
  }
}