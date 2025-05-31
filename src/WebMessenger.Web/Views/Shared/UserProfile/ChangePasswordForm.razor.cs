using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;

namespace WebMessenger.Web.Views.Shared.UserProfile;

public partial class ChangePasswordForm : ComponentBase
{
  [Inject] private IAccountApi AccountApi { get; set; } = null!;
  
  private ChangePasswordModel _model = new();
  
  private EditContext _editContext = null!;
  private ValidationMessageStore _invalidMessageStore = null!;
  
  private bool _isLoading;
  private bool _isSuccess;
  
  protected override void OnInitialized()
  {
    _editContext = new EditContext(_model);
    _invalidMessageStore = new ValidationMessageStore(_editContext);
    
    _editContext.OnFieldChanged += (_, args) =>
    {
      _invalidMessageStore.Clear(args.FieldIdentifier);
      _editContext.NotifyValidationStateChanged();
    };
  }
  
  private async Task ChangePassword()
  {
    _isLoading = true;
    _isSuccess = false;

    await HttpHelper.FetchAsync(async () => await AccountApi.ChangePasswordAsync(new ChangePasswordDto
      {
        OldPassword = _model.OldPassword,
        NewPassword = _model.NewPassword
      }),
      onSuccess: _ =>
      {
        _model = new ChangePasswordModel();
        _isSuccess = true;
        
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
          var fieldIdentifier = new FieldIdentifier(_model, "OldPassword");
          _invalidMessageStore.Add(fieldIdentifier, displayError);
        }
        
        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
          var fieldIdentifier = new FieldIdentifier(_model, "NewPassword");
          _invalidMessageStore.Add(fieldIdentifier, displayError);
        }
      },
      onException: exception =>
      {
        
      });
    
    _isLoading = false;
  }
}