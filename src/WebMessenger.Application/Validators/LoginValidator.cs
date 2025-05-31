using FluentValidation;
using WebMessenger.Application.Common;
using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.Application.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
  public LoginValidator()
  {
    RuleLevelCascadeMode = CascadeMode.Stop;
    
    RuleFor(dto => dto.Email)
      .NotEmpty().WithMessage(ErrorMessages.IsRequired)
      .EmailAddress().WithMessage(ErrorMessages.InvalidFormat);
    
    RuleFor(dto => dto.Password)
      .NotEmpty().WithMessage(ErrorMessages.IsRequired)
      .MinimumLength(6).WithMessage(ErrorMessages.MinSize(6))
      .Matches("[A-Z]").WithMessage(ErrorMessages.MustContainUppercaseLetter)
      .Matches("[0-9]").WithMessage(ErrorMessages.MustContainNumber)
      .Matches("[^a-zA-Z0-9]").WithMessage(ErrorMessages.MustContainSpecialCharacter);
  }
}