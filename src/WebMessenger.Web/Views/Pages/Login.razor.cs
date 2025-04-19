using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebMessenger.Shared.Models;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared;

namespace WebMessenger.Web.Views.Pages;

public partial class Login : ComponentBase
{
  [Inject] private IAuthApiSource AuthApiSource { get; set; }
  [Inject] private IAuthState AuthState { get; set; }
  [Inject] private NavigationManager NavManager { get; set; }
  
  private readonly LoginModel _model = new();
  private bool _isLoading;
  private string? _error;
  
  private async Task LoginAsync(AuthForm<LoginModel>.SubmitCallbackArgs args)
  {
    _isLoading = true;
    _error = null;

    await HttpHelper.FetchAsync(() => AuthApiSource.LoginAsync(new LoginRequest
      {
        Email = _model.Email ?? string.Empty,
        Password = _model.Password ?? string.Empty,
        RememberMe = _model.RememberMe
      }),
      onSuccess: async response =>
      {
        var authResult = await response.Content.ReadFromJsonAsync<AuthResult>();
        if (authResult == null)
          throw new NullReferenceException(nameof(authResult));
        
        AuthState.Authenticate(authResult.Token);
        NavManager.NavigateTo("/");
      },
      onFailure: async response =>
      {
        var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        if (errors == null)
          throw new NullReferenceException(nameof(errors));

        foreach (var error in errors)
        {
          if (error.Value == "ACCOUNT_NOT_ACTIVE")
          {
            NavManager.NavigateTo($"auth/activation?email={_model.Email}");
          }

          var correctedKey = char.ToUpper(error.Key[0]) + error.Key[1..];
          var displayError = ErrorToDisplayMessageMapper.ToDisplayMessage(error.Value);

          if (correctedKey == "Other")
          {
            _error = displayError;
            continue;
          }

          var fieldIdentifier = new FieldIdentifier(_model, correctedKey);
          args.ValidationMessageStore.Add(fieldIdentifier, displayError);
        }

        args.EditContext.NotifyValidationStateChanged();
      },
      onException: exception =>
      {
        var displayError = ErrorToDisplayMessageMapper.ToDisplayMessage(exception.Message);
        _error = displayError;
      });
    
    _isLoading = false;
  }
}