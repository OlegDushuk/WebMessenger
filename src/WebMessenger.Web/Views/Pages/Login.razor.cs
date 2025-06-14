using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared.Auth;

namespace WebMessenger.Web.Views.Pages;

public partial class Login : ComponentBase
{
  [Inject] private IAuthApi AuthApi { get; set; } = null!;
  [Inject] private IAuthState AuthState { get; set; } = null!;
  [Inject] private NavigationManager NavManager { get; set; } = null!;

  private readonly LoginModel _model = new();
  private bool _isLoading;
  private string? _error;
  
  private async Task LoginAsync(AuthForm<LoginModel>.SubmitCallbackArgs args)
  {
    _isLoading = true;
    _error = null;

    await HttpHelper.FetchAsync(() => AuthApi.LoginAsync(new LoginDto
      {
        Email = _model.Email ?? string.Empty,
        Password = _model.Password ?? string.Empty,
        RememberMe = _model.RememberMe
      }),
      onSuccess: async response =>
      {
        var authResult = await response.Content.ReadFromJsonAsync<AuthDto>();
        if (authResult == null)
          throw new NullReferenceException(nameof(authResult));
        
        await AuthState.Authenticate(authResult.AccessToken);
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