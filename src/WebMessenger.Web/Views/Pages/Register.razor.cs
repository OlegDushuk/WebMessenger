using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared.Auth;

namespace WebMessenger.Web.Views.Pages;

public partial class Register : ComponentBase
{
  [Inject] private IAuthApi AuthApi { get; set; } = null!;
  [Inject] private NavigationManager NavManager { get; set; } = null!;

  private readonly RegisterModel _model = new();
  private bool _isLoading;
  private string? _error;
  
  private async Task RegisterAsync(AuthForm<RegisterModel>.SubmitCallbackArgs args)
  {
    _isLoading = true;
    _error = null;
    
    await HttpHelper.FetchAsync(() => AuthApi.RegisterAsync(new RegisterDto
      {
        Name = _model.Name ?? string.Empty,
        UserName = _model.UserName ?? string.Empty,
        Email = _model.Email ?? string.Empty,
        Password = _model.Password ?? string.Empty,
      }),
      onSuccess: _ =>
      {
        NavManager.NavigateTo($"auth/activation?email={_model.Email}");
        return Task.CompletedTask;
      },
      onFailure: async response =>
      {
        var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        if (errors == null)
          throw new NullReferenceException(nameof(errors));
        
        foreach (var error in errors)
        {
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
        _isLoading = false;
      },
      onException: exception =>
      {
        var displayError = ErrorToDisplayMessageMapper.ToDisplayMessage(exception.Message);
        _error = displayError;
      });
  }
}