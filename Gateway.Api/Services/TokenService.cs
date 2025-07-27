using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gateway.Api.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string GenerateToken(string username, string role)
    {
        var audiences = config.GetSection("Jwt:Audiences").Get<string[]>()!;
        if (audiences is null || audiences.Length == 0)
            throw new InvalidOperationException("JWT configuration error: Either a valid tenantId must be provided or at least one audience must be configured.");

        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("JWT secret not found in environment or configuration.");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        // Add multiple audiences as separate claims (same key)
        claims.AddRange(audiences.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(config["Jwt:ExpiryMinutes"])),
            signingCredentials: creds
        );


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}