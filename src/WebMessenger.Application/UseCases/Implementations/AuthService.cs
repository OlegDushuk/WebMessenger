using WebMessenger.Application.Common;
using WebMessenger.Application.Common.Enums;
using WebMessenger.Application.Common.Models;
using WebMessenger.Application.DTOs.Requests;
using WebMessenger.Application.DTOs.Responses;
using WebMessenger.Application.UseCases.Interfaces;
using WebMessenger.Application.Validators;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Repositories;
using WebMessenger.Core.Interfaces.Services;

namespace WebMessenger.Application.UseCases.Implementations;

public class AuthService(
  RegisterValidator regValidator,
  EmailValidator emailValidator,
  PasswordValidator passwordValidator,
  LoginValidator loginValidator,
  IUserRepository userRepository,
  IEmailVerificationTokenRepository verTokenRepository,
  IRefreshTokenRepository refTokenRepository,
  IResetPasswordTokenRepository resetPasswordTokenRepository,
  IEmailService emailService,
  IAuthTokenService authTokenService,
  IPasswordService passwordService
  ) : IAuthService
{
  public async Task<Result> RegisterAsync(RegisterDto dto)
  {
    var validationResult = await regValidator.ValidateAsync(dto);
    if (!validationResult.IsValid)
    {
      var errorDictionary = validationResult.Errors
        .GroupBy(x => x.PropertyName)
        .ToDictionary(
          g => g.Key.ToLower(),
          g => g.First().ErrorMessage
        );
      
      return Result.Failure(
        ErrorType.Validation,
        errorDictionary
      );
    }

    if (await userRepository.ExistsByEmailAsync(dto.Email))
      return Result.Failure(
        ErrorType.Conflict,
        new
        {
          Email = ErrorMessages.EmailAlreadyInUse
        });

    if (await userRepository.ExistsByUserNameAsync(dto.UserName))
      return Result.Failure(
        ErrorType.Conflict,
        new
        {
          Username = ErrorMessages.UserNameAlreadyInUse
        });
    
    var user = new User
    {
      UserName = dto.UserName,
      Email = dto.Email,
      PasswordHash = passwordService.HashPassword(dto.Password),
      Name = dto.Name,
      IsActive = false,
    };
    var id = await userRepository.CreateAsync(user);
    
    var verificationToken = new EmailVerificationToken
    {
      UserId = id,
      Token = Guid.NewGuid().ToString("N"),
      ExpiresAt = DateTime.UtcNow.AddDays(2),
    };
    await verTokenRepository.CreateAsync(verificationToken);
    
    await emailService.SendEmailVerificationAsync(user.Email, verificationToken.Token);
    
    return Result.Success;
  }
  
  public async Task<Result> VerifyEmailAsync(string token)
  {
    if (string.IsNullOrEmpty(token))
      return Result.Failure(
        ErrorType.Validation,
        ErrorMessages.IsRequired
      );
    
    var verToken = await verTokenRepository.GetByTokenAsync(token);
    if (verToken == null)
      return Result.Failure(
        ErrorType.NotFound,
        ErrorMessages.EntityNotFound("Token")
      );
    
    if (verToken.ExpiresAt < DateTime.UtcNow)
      return Result.Failure(
        ErrorType.Conflict,
        ErrorMessages.TimeExpired
      );
    
    var user = await userRepository.GetByIdAsync(verToken.UserId);
    if (user == null)
    {
      await verTokenRepository.DeleteAsync(verToken.Id);
      
      return Result.Failure(
        ErrorType.NotFound,
        ErrorMessages.EntityNotFound("User")
      );
    }
    
    user.IsActive = true;
    await userRepository.UpdateAsync(user);
    await verTokenRepository.DeleteAsync(verToken.Id);

    return Result.Success;
  }
  
  public async Task<Result> ResendVerifyAsync(string email)
  {
    var validationResult = await emailValidator.ValidateAsync(email);
    if (!validationResult.IsValid)
    {
      return Result.Failure(
        ErrorType.Validation,
        validationResult.Errors.First().ErrorMessage
      );
    }
    
    var user = await userRepository.GetByEmailAsync(email);
    if (user == null)
      return Result.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );
    
    if (user.IsActive)
      return Result.Failure(
        ErrorType.Conflict,
        "USER_IS_ACTIVE"
      );

    var existsToken = await verTokenRepository.GetByUserIdAsync(user.Id);
    if (existsToken != null)
      await verTokenRepository.DeleteAsync(existsToken.Id);
    
    var verificationToken = new EmailVerificationToken
    {
      UserId = user.Id,
      Token = Guid.NewGuid().ToString(),
      ExpiresAt = DateTime.UtcNow.AddDays(2),
    };
    await verTokenRepository.CreateAsync(verificationToken);
    
    await emailService.SendEmailVerificationAsync(email, verificationToken.Token);
    
    return Result.Success;
  }
  
  public async Task<Result<AuthDto>> LoginAsync(LoginDto dto)
  {
    var validationResult = await loginValidator.ValidateAsync(dto);
    if (!validationResult.IsValid)
    {
      var errorDictionary = validationResult.Errors
        .GroupBy(e => e.PropertyName)
        .ToDictionary(
          g => g.Key.ToLower(),
          g => g.First().ErrorMessage
        );
      
      return Result<AuthDto>.Failure(
        ErrorType.Validation,
        errorDictionary
      );
    }
    
    var user = await userRepository.GetByEmailAsync(dto.Email);
    if (user == null)
      return Result<AuthDto>.Failure(
        ErrorType.NotFound,
        new
        {
          Email = "USER_BY_THIS_EMAIL_NOT_FOUND"
        });
    
    if (!user.IsActive)
      return Result<AuthDto>.Failure(
        ErrorType.Conflict,
        new
        {
          Other = "ACCOUNT_NOT_ACTIVE"
        });
    
    if (!passwordService.VerifyPassword(dto.Password, user.PasswordHash!))
      return Result<AuthDto>.Failure(
        ErrorType.Conflict,
        new
        {
          Password = "PASSWORD_INVALID"
        });
    
    var accessToken = authTokenService.GenerateAccessToken(user);
    var auth = new AuthDto
    {
      AccessToken = accessToken,
    };
    
    if (dto.RememberMe)
    {
      var refreshToken = authTokenService.GenerateRefreshToken(user.Id);
      await refTokenRepository.CreateAsync(refreshToken);
      auth.RefreshToken = refreshToken.Token;
    }
    
    return Result<AuthDto>.Success(auth);
  }
  
  public async Task<Result<string>> RefreshTokenAsync(string token)
  {
    var refToken = await refTokenRepository.GetByTokenAsync(token);
    if (refToken == null)
      return Result<string>.Failure(ErrorType.NotFound, "Token not found");
    
    if (refToken.ExpiresAt < DateTime.UtcNow)
    {
      await refTokenRepository.DeleteAsync(refToken.Id);
      return Result<string>.Failure(ErrorType.Conflict, "Token is expired");
    }
    
    var user = await userRepository.GetByIdAsync(refToken.UserId);
    if (user == null)
    {
      await refTokenRepository.DeleteAsync(refToken.Id);
      return Result<string>.Failure(ErrorType.NotFound, "User by this token not found");
    }
    
    var accessToken = authTokenService.GenerateAccessToken(user);
    
    return Result<string>.Success(accessToken);
  }
  
  public async Task<Result> RevokeTokenAsync(string token)
  {
    var refToken = await refTokenRepository.GetByTokenAsync(token);
    if (refToken == null)
      return Result.Failure(ErrorType.NotFound, "TOKEN_NOT_FOUND");
    
    await refTokenRepository.DeleteAsync(refToken.Id);
    return Result.Success;
  }
  
  public async Task<Result> ForgotPasswordAsync(string email)
  {
    var validationResult = await emailValidator.ValidateAsync(email);
    if (!validationResult.IsValid)
      return Result.Failure(
        ErrorType.Validation,
        validationResult.Errors[0].ErrorMessage
      );
    
    var user = await userRepository.GetByEmailAsync(email);
    if (user == null)
      return Result<UserDto>.Failure(
        ErrorType.NotFound,
        "USER_BY_THIS_EMAIL_NOT_FOUND"
      );
    
    var token = new ResetPasswordToken
    {
      UserId = user.Id,
      Token = Guid.NewGuid().ToString("N"),
      ExpiresAt = DateTime.UtcNow.AddDays(2),
    };
    await resetPasswordTokenRepository.CreateAsync(token);
    await emailService.SendPasswordResetAsync(email, token.Token);
    
    return Result.Success;
  }
  
  public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto)
  {
    var validationResult = await passwordValidator.ValidateAsync(dto.NewPassword);
    if (!validationResult.IsValid)
    {
      return Result.Failure(
        ErrorType.Validation,
        validationResult.Errors[0].ErrorMessage
      );
    }
    
    var resToken = await resetPasswordTokenRepository.GetByTokenAsync(dto.Token);
    if (resToken == null)
      return Result.Failure(ErrorType.NotFound, "TOKEN_NOT_FOUND");
    
    var user = await userRepository.GetByIdAsync(resToken.UserId);
    if (user == null)
    {
      await resetPasswordTokenRepository.DeleteAsync(resToken);
      return Result.Failure(ErrorType.NotFound, "USER_BY_THIS_TOKEN_NOT_FOUND");
    }
    
    user.PasswordHash = passwordService.HashPassword(dto.NewPassword);
    await userRepository.UpdateAsync(user);
    await resetPasswordTokenRepository.DeleteAsync(resToken);
    
    return Result.Success;
  }
}