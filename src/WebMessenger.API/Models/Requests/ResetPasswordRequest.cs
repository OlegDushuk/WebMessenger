﻿namespace WebMessenger.API.Models.Requests;

public class ResetPasswordRequest
{
  public string Token { get; set; } = string.Empty;
  public string NewPassword { get; set; } = string.Empty;
}