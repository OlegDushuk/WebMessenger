using FluentValidation;
using WebMessenger.Application.Common;

namespace WebMessenger.Application.Validators;

public class EmailValidator : AbstractValidator<string>
{
  public EmailValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(email => email)
      .NotEmpty().WithMessage(ErrorMessages.IsRequired)
      .EmailAddress().WithMessage(ErrorMessages.InvalidFormat);
  }
}