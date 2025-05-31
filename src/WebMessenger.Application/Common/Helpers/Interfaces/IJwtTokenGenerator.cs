using WebMessenger.Core.Entities;

namespace WebMessenger.Application.Common.Helpers.Interfaces;

public interface IJwtTokenGenerator
{
  string GenerateAccessToken(User user);
}