namespace WebMessenger.Infrastructure.ClientConnection;

public class ConnectionClientStorage
{
  private readonly Dictionary<Guid, string> _connections = new();
  
  public void AddUser(Guid userId, string connectionId)
  {
    _connections.Add(userId, connectionId);
  }
  
  public void RemoveUser(Guid userId)
  {
    _connections.Remove(userId);
  }

  public void RemoveUser(string connectionId)
  {
    var item = _connections.FirstOrDefault(kv => kv.Value == connectionId);
    _connections.Remove(item.Key);
  }
  
  public string? GetConnectionId(Guid userId)
  {
    return _connections.TryGetValue(userId, out var value) ? value : null;
  }
}