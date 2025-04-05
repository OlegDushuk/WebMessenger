﻿using WebMessenger.Core.Interfaces.Services;
using System;

namespace WebMessenger.Infrastructure.ExternalServices;

public class PasswordService : IPasswordService
{
  private const int WorkFactor = 12;
  
  public string HashPassword(string password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
  }
  
  public bool VerifyPassword(string password, string hashedPassword)
  {
    return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
  }
}