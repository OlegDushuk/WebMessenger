using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebMessenger.Application.Common.Helpers.Interfaces;
using WebMessenger.Application.Common.Helpers.Settings;
using WebMessenger.Core.Entities;

namespace WebMessenger.Application.Common.Helpers.Implementations;

public class JwtTokenGenerator(IOptions<JwtSettings> settings) :  IJwtTokenGenerator
{
  public string GenerateAccessToken(User user)
  {
    var jwtSettings = settings.Value;
    
    var claims = new List<Claim>
    {
      new(JwtRegisteredClaimNames.Email, user.Email!),
    };
    
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow.AddMinutes(jwtSettings.Expiration);
    
    var token = new JwtSecurityToken(
      claims: claims,
      expires: expires,
      signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}