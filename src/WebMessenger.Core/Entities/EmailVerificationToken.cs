﻿namespace WebMessenger.Core.Entities;

public class EmailVerificationToken
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public string? Token { get; set; }
  public DateTime ExpiresAt { get; set; }
}