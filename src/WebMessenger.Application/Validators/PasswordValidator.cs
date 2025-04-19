using FluentValidation;
using WebMessenger.Application.Common;

namespace WebMessenger.Application.Validators;

public class PasswordValidator : AbstractValidator<string>
{
  public PasswordValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(password => password)
      .NotEmpty().WithMessage(ErrorMessages.IsRequired)
      .MinimumLength(6).WithMessage(ErrorMessages.MinSize(6))
      .Matches("[A-Z]").WithMessage(ErrorMessages.MustContainUppercaseLetter)
      .Matches("[0-9]").WithMessage(ErrorMessages.MustContainNumber)
      .Matches("[^a-zA-Z0-9]").WithMessage(ErrorMessages.MustContainSpecialCharacter);
  }
}