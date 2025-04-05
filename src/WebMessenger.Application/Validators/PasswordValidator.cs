using FluentValidation;

namespace WebMessenger.Application.Validators;

public class PasswordValidator : AbstractValidator<string>
{
  public PasswordValidator()
  {
    RuleFor(p => p)
      .NotEmpty().WithMessage("Password is required")
      .MinimumLength(6).WithMessage("Password must be at least 6 characters")
      .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
      .Matches("[0-9]").WithMessage("Password must contain at least one number")
      .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
  }
}