using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebMessenger.Shared.Models;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared;

namespace WebMessenger.Web.Views.Pages;

public partial class ResetPassword : ComponentBase
{
  [Inject] private IAuthApiSource AuthApiSource { get; set; }
  
  [Parameter] public string Token { get; set; } = string.Empty;
  
  private readonly ResetPasswordModel _model = new();
  private bool _isLoading;
  private bool _isSuccess;
  private string? _error;
  
  private async Task ResetPasswordAsync(AuthForm<ResetPasswordModel>.SubmitCallbackArgs args)
  {
    _isLoading = true;
    _isSuccess = false;

    await HttpHelper.FetchAsync(async () => await AuthApiSource.ResetPasswordAsync(new ResetPasswordRequest
      {
        Token = Token,
        NewPassword = _model.NewPassword ?? "",
      }),
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

        if (response.StatusCode == HttpStatusCode.UnprocessableContent)
        {
          var fieldIdentifier = new FieldIdentifier(_model, "NewPassword");
          args.ValidationMessageStore.Add(fieldIdentifier, displayError);
          args.EditContext.NotifyValidationStateChanged();
        }
        
        if (error is "TOKEN_NOT_FOUND" or "USER_BY_THIS_TOKEN_NOT_FOUND")
          _error =
            "Термін дії посилання за яким ви зайшли відновити пароль вичерпався, або посилання не дійсне. " +
            "Спробуйте надіслати лист знову";
        else
          _error = displayError;
      },
      onException: exception =>
      {
        var displayError = ErrorToDisplayMessageMapper.ToDisplayMessage(exception.Message);
        _error = displayError;
      });
    
    _isLoading = false;
  }
}