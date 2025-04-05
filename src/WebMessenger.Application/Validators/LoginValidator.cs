﻿using FluentValidation;
using WebMessenger.Application.DTOs.Requests;

namespace WebMessenger.Application.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
  public LoginValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email is required")
      .EmailAddress().WithMessage("Email must be a valid email address");
    
    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("Password is required")
      .MinimumLength(6).WithMessage("Password must be at least 6 characters")
      .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
      .Matches("[0-9]").WithMessage("Password must contain at least one number")
      .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
  }
}