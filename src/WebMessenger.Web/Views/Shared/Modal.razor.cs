using Microsoft.AspNetCore.Components;

namespace WebMessenger.Web.Views.Shared;

public partial class Modal : ComponentBase
{
  public class ModalController
  {
    public bool IsShow { get; set; }
  }
  
  [Parameter] public ModalController? Controller { get; set; }
  [Parameter] public RenderFragment? ChildContent { get; set; }
  
  private bool IsShow => Controller?.IsShow ?? false;
  
  private void HandleCloseClick()
  {
    if (Controller != null)
      Controller.IsShow = false;
  }
}