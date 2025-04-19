using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using WebMessenger.Shared.Models;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;

namespace WebMessenger.Web.Views.Shared;

public partial class ProfileModal : ComponentBase
{
  [Inject] private IAuthApiSource AuthApiSource { get; set; }
  [Inject] private IAccountApiSource AccountApiSource { get; set; }
  [Inject] private IAuthState AuthState { get; set; }
  [Inject] private NavigationManager NavManager { get; set; }
  [Inject] private IJSRuntime Js { get; set; }
  
  [Parameter] public Modal.ModalController? ModalController { get; set; }
  
  private UpdateAccountDataModel _updateAccountDataModel = new();
  
  private EditContext _passwordChangeFormEditContext;
  private ValidationMessageStore _passwordChangeFormMessageStore;
  private ChangePasswordModel _changePasswordModel = new();

  private bool _isShowSettings;
  
  private bool _isLoadingAccountData;
  private bool _isSuccessAccountData;
  
  private bool _isLoadingPassword;
  private bool _isSuccessPassword;
  
  protected override void OnInitialized()
  {
    _passwordChangeFormEditContext = new EditContext(_changePasswordModel);
    _passwordChangeFormMessageStore = new ValidationMessageStore(_passwordChangeFormEditContext);
    
    _passwordChangeFormEditContext.OnFieldChanged += (_, args) =>
    {
      _passwordChangeFormMessageStore.Clear(args.FieldIdentifier);
      _passwordChangeFormEditContext.NotifyValidationStateChanged();
    };
  }
  
  private void SwitchSettings()
  {
    _isShowSettings = !_isShowSettings;
  }
  
  private async Task HandleLogout()
  {
    await HttpHelper.FetchAsync(async () => await AuthApiSource.RevokeTokenAsync(),
      onSuccess: response =>
      {
        return Task.CompletedTask;
      },
      onFailure: response =>
      {
        return Task.CompletedTask;
      },
      onException: exception =>
      {
        
      });
    
    AuthState.Logout();
    NavManager.NavigateTo("/auth/login");
  }

  private async Task SaveAccountData()
  {
    _isLoadingAccountData = true;
    _isSuccessAccountData = false;

    await HttpHelper.FetchAsync(async () => await AccountApiSource.UpdateAccountDataAsync(new UpdateAccountDataRequest
      {
        UserName = _updateAccountDataModel.UserName,
        Name = _updateAccountDataModel.Name,
        Bio = _updateAccountDataModel.Bio,
      }),
      onSuccess: _ =>
      {
        if (_updateAccountDataModel.UserName != null)
          UserState.User!.UserName = _updateAccountDataModel.UserName;
        
        if (_updateAccountDataModel.Name != null)
          UserState.User!.Name = _updateAccountDataModel.Name;
        
        if (_updateAccountDataModel.Bio != null)
          UserState.User!.Bio = _updateAccountDataModel.Bio;
        
        _isSuccessAccountData = true;
        
        return Task.CompletedTask;
      },
      onFailure: response =>
      {
        return Task.CompletedTask;
      },
      onException: exception =>
      {
        
      });
    
    _isLoadingAccountData = false;
  }
  
  private async Task ChangePassword()
  {
    _isLoadingPassword = true;
    _isSuccessPassword = false;

    await HttpHelper.FetchAsync(async () => await AccountApiSource.ChangePasswordAsync(new ChangePasswordRequest
      {
        OldPassword = _changePasswordModel.OldPassword,
        NewPassword = _changePasswordModel.NewPassword
      }),
      onSuccess: _ =>
      {
        _isSuccessPassword = true;
        
        return Task.CompletedTask;
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));
        
        var displayError = ErrorToDisplayMessageMapper.ToDisplayMessage(error);
        
        if (response.StatusCode == HttpStatusCode.Conflict)
        {
          var fieldIdentifier = new FieldIdentifier(_changePasswordModel, "OldPassword");
          _passwordChangeFormMessageStore.Add(fieldIdentifier, displayError);
        }
        
        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
          var fieldIdentifier = new FieldIdentifier(_changePasswordModel, "NewPassword");
          _passwordChangeFormMessageStore.Add(fieldIdentifier, displayError);
        }
      },
      onException: exception =>
      {
        
      });
    
    _isLoadingPassword = false;
  }

  private async Task DeleteAccount()
  {
    var confirmed = await Js.InvokeAsync<bool>("confirmDelete", "Ви впевнені, що хочете видалити акаунт?");
    if (confirmed)
    {
      await HttpHelper.FetchAsync(async () => await AccountApiSource.DeleteUserAsync(),
        onSuccess: _ =>
        {
          NavManager.NavigateTo("/auth/login");
          
          return Task.CompletedTask;
        },
        onFailure: async response =>
        {
          
        },
        onException: exception =>
        {
        
        });
    }
  }
}