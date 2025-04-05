﻿namespace WebMessenger.API.Models.Requests;

public class RegisterRequest
{
  public string Name { get; set; } = string.Empty;
  public string UserName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}