using Microsoft.AspNetCore.Components;

namespace WebMessenger.Web.Views.Shared.Modal;

public partial class ModalView : ComponentBase
{
  private bool _isShow;
  private string? _title;
  private RenderFragment? _content;
  
  public void Open(string? title, RenderFragment? content)
  {
    _isShow = true;
    _title = title;
    _content = content;
    InvokeAsync(StateHasChanged);
  }
  
  public void Close()
  {
    _isShow = false;
    _title = null;
    _content = null;
    InvokeAsync(StateHasChanged);
  }
}