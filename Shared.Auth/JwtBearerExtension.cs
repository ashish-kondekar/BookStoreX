using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Shared.Auth;

public static class JwtBearerExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var issuer = jwtSettings["Issuer"];
        //var audience = jwtSettings["Audience"];
        var audienceList = jwtSettings.GetSection("Audiences").Get<string[]>();
        var secret = jwtSettings["Key"];

        if (string.IsNullOrEmpty(secret))
            throw new InvalidOperationException("JWT secret not found in environment or configuration.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudiences = audienceList,
                        IssuerSigningKey = key
                    };
                });
        return services;
    }
}
