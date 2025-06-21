namespace WebMessenger.Web.Services;

public class ChatViewState
{
  private bool _isShowing;
  
  public Action? OnStateChanged { get; set; }

  public bool IsShowing
  {
    get => _isShowing;
    set
    {
      _isShowing = value;
      OnStateChanged?.Invoke();
    }
  }
}