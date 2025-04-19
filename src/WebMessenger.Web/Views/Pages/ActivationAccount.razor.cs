using Microsoft.AspNetCore.Components;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared;

namespace WebMessenger.Web.Views.Pages;

public partial class ActivationAccount : ComponentBase
{
  [SupplyParameterFromQuery] public string? Email { get; set; }
  
  [Inject] private IAuthApiSource AuthApiSource { get; set; }

  private readonly EmailModel _model = new();
  private bool _isLoading;
  private bool _isSuccess;
  private string? _error;
  
  private async Task SendEmail(AuthForm<EmailModel>.SubmitCallbackArgs args)
  {
    _isLoading = true;
    _isSuccess = false;
    _error = null;
    
    await HttpHelper.FetchAsync(async () => await AuthApiSource.SendVerificationLinkAsync(_model.Email ?? ""),
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