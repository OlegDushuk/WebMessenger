namespace WebMessenger.Infrastructure.ClientConnection;

public class ConnectionClientStorage
{
  private class UserConnection(Guid userId, string connectionId)
  {
    public Guid UserId { get; } = userId;
    public string ConnectionId { get; } = connectionId;
  }
  
  private readonly List<UserConnection> _connections = [];
  
  public void AddUser(Guid userId, string connectionId)
  {
    _connections.Add(new UserConnection(userId, connectionId));
  }

  public void RemoveUser(string connectionId)
  {
    var item = _connections.FirstOrDefault(uc => uc.ConnectionId == connectionId);
    if (item != null)
      _connections.Remove(item);
  }
  
  public string? GetConnectionId(Guid userId)
  {
    return _connections.FirstOrDefault(uc => uc.UserId == userId)?.ConnectionId;
  }
}