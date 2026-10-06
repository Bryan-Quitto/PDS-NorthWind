using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NorthWind.Membership.Backend.Core.Dtos;
using NorthWind.Membership.Backend.Core.Options;
using System.Security.Claims;
using System.Text;

namespace NorthWind.Membership.Backend.Core.Services;

public class JwtService(IOptions<JwtOptions> jwtOptions)
{
    private readonly JwtOptions options = jwtOptions.Value;

    public string GenerateToken(UserDto user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Dictionary<string, object>
        {
            [ClaimTypes.NameIdentifier] = user.Id,
            [ClaimTypes.Name] = user.UserName,
            [ClaimTypes.Email] = user.Email,
            ["FullName"] = user.FullName
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = options.Issuer,
            Audience = options.Audience,
            Claims = claims,
            Expires = DateTime.UtcNow.AddMinutes(options.ExpirationMinutes),
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(descriptor);
    }
}
