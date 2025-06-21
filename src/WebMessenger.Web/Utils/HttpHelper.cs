namespace WebMessenger.Web.Utils;

public static class HttpHelper
{
  public static async Task FetchAsync(
    Func<Task<HttpResponseMessage>> request,
    Func<HttpResponseMessage, Task>? onSuccess = null,
    Func<HttpResponseMessage, Task>? onFailure = null,
    Action<Exception>? onException = null)
  {
    try
    {
      var response = await request.Invoke();
      
      if (response.IsSuccessStatusCode)
      {
        onSuccess?.Invoke(response);
        return;
      }
      
      onFailure?.Invoke(response);
    }
    catch (Exception e)
    {
      onException?.Invoke(e);
    }
  }
}