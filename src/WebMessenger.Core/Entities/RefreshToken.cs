﻿namespace WebMessenger.Core.Entities;

public class RefreshToken
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public string Token { get; set; } = string.Empty;
  public DateTime ExpiresAt { get; set; }
}