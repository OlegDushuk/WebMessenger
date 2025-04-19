using WebMessenger.Application.Common.Models;
using WebMessenger.Application.DTOs.Requests;
using WebMessenger.Application.DTOs.Responses;

namespace WebMessenger.Application.UseCases.Interfaces;

public interface IAccountService
{
  Task<Result<UserDto>> GetUserAsync(string email);
  Task<Result> UpdateAccountDataAsync(string email, UpdateAccountDataDto dto);
  Task<Result> ChangePasswordAsync(string email, ChangePasswordDto dto);
  Task<Result> DeleteUserAsync(string email);
}