using FluentValidation;

namespace WebMessenger.Application.Validators;

public class EmailValidator : AbstractValidator<string>
{
  public EmailValidator()
  {
    RuleFor(x => x)
      .NotEmpty().WithMessage("Email is required")
      .EmailAddress().WithMessage("Email is invalid");
  }
}