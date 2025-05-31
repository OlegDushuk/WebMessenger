using Microsoft.AspNetCore.Components;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;

namespace WebMessenger.Web.Views.Shared.UserProfile;

public partial class UpdateAccountForm : ComponentBase
{
  [Inject] private IAccountApi AccountApi { get; set; } = null!;
  [Inject] private IUserState UserState { get; set; } = null!;
  
  private UpdateAccountDataModel _model = new();
  
  private bool _isLoading;
  private bool _isSuccess;
  
  private async Task SaveData()
  {
    _isLoading = true;
    _isSuccess = false;

    await HttpHelper.FetchAsync(async () => await AccountApi.UpdateAccountDataAsync(new UpdateAccountDataDto
      {
        UserName = _model.UserName,
        Name = _model.Name,
        Bio = _model.Bio,
      }),
      onSuccess: _ =>
      {
        if (_model.UserName != null)
          UserState.User!.UserName = _model.UserName;
        
        if (_model.Name != null)
          UserState.User!.Name = _model.Name;
        
        if (_model.Bio != null)
          UserState.User!.Bio = _model.Bio;

        _model = new UpdateAccountDataModel();
        _isSuccess = true;
        
        return Task.CompletedTask;
      },
      onFailure: response =>
      {
        return Task.CompletedTask;
      },
      onException: exception =>
      {
        
      });
    
    _isLoading = false;
  }
}