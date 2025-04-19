using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Interfaces.Services;

namespace WebMessenger.Infrastructure.ExternalServices;

public class JwtSettings
{
  public string Secret { get; set; } = string.Empty;
  public int Expiration { get; set; }
}

public class AuthTokenService(IOptions<JwtSettings> settings) : IAuthTokenService
{
  private readonly JwtSettings _jwtSettings = settings.Value;
  
  public string GenerateAccessToken(User user)
  {
    var claims = new List<Claim>
    {
      new(JwtRegisteredClaimNames.Email, user.Email!),
    };
    
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.Expiration);
    
    var token = new JwtSecurityToken(
      claims: claims,
      expires: expires,
      signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  public RefreshToken GenerateRefreshToken(Guid userId)
  {
    return new RefreshToken
    {
      UserId = userId,
      Token = Guid.NewGuid().ToString("N"),
      ExpiresAt = DateTime.UtcNow.AddDays(7),
    };
  }
}