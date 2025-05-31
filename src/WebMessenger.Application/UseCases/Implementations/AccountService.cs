using AutoMapper;
using WebMessenger.Application.Common.Enums;
using WebMessenger.Application.Common.Helpers;
using WebMessenger.Application.Common.Models;
using WebMessenger.Application.UseCases.Interfaces;
using WebMessenger.Application.Validators;
using WebMessenger.Core.Interfaces.Repositories;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Application.UseCases.Implementations;

public class AccountService(
  EmailValidator emailValidator,
  PasswordValidator passwordValidator,
  IUserRepository userRepository,
  IMapper mapper
) : IAccountService
{
  public async Task<Result<UserDto>> GetUserAsync(string email)
  {
    var validationResult = await emailValidator.ValidateAsync(email);
    if (!validationResult.IsValid)
      return Result<UserDto>.Failure(
        ErrorType.Validation,
        validationResult.Errors[0].ErrorMessage
      );
    
    var user = await userRepository.GetByEmailAsync(email);
    if (user == null)
      return Result<UserDto>.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );
    
    var userDto = mapper.Map<UserDto>(user);
    
    return Result<UserDto>.Success(userDto);
  }
  
  public async Task<Result> UpdateAccountDataAsync(string email, UpdateAccountDataDto dto)
  {
    var user = await userRepository.GetByEmailAsync(email);
    if (user == null)
      return Result.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );
    
    if (dto.UserName != null)
      user.UserName = dto.UserName;
    
    if (dto.Name != null)
      user.Name = dto.Name;
    
    if (dto.Bio != null)
      user.Bio = dto.Bio;

    await userRepository.UpdateAsync(user);

    return Result.Success;
  }
  
  public async Task<Result> ChangePasswordAsync(string email, ChangePasswordDto dto)
  {
    var user = await userRepository.GetByEmailAsync(email);
    if (user == null)
      return Result.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );

    if (!PasswordHasher.VerifyPassword(dto.OldPassword, user.PasswordHash!))
      return Result.Failure(
        ErrorType.Conflict,
        "PASSWORD_INVALID"
      );
    
    var validationResult = await passwordValidator.ValidateAsync(dto.NewPassword);
    if (!validationResult.IsValid)
    {
      return Result.Failure(
        ErrorType.Validation,
        validationResult.Errors[0].ErrorMessage
      );
    }
    
    user.PasswordHash = PasswordHasher.HashPassword(dto.NewPassword);
    await userRepository.UpdateAsync(user);
    
    return Result.Success;
  }

  public async Task<Result> DeleteUserAsync(string email)
  {
    var validationResult = await emailValidator.ValidateAsync(email);
    if (!validationResult.IsValid)
      return Result.Failure(
        ErrorType.Validation,
        validationResult.Errors[0].ErrorMessage
      );
    
    var user = await userRepository.GetByEmailAsync(email);
    if (user == null)
      return Result.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );

    await userRepository.DeleteAsync(user);
    
    return Result.Success;
  }
}