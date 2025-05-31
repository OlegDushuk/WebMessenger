using WebMessenger.Application.Common.Models;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Application.UseCases.Interfaces;

public interface IAccountService
{
  Task<Result<UserDto>> GetUserAsync(string email);
  Task<Result> UpdateAccountDataAsync(string email, UpdateAccountDataDto dto);
  Task<Result> ChangePasswordAsync(string email, ChangePasswordDto dto);
  Task<Result> DeleteUserAsync(string email);
}