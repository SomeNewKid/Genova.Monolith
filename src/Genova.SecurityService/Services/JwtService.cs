using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Genova.SecurityService.Services;

public class JwtService : IJwtService {
    private readonly string _secretKey = "super-secret-key"; // Load from configuration

    public string GenerateToken(string username, List<string> roles) 
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.UTF8.GetBytes(_secretKey);

        List<Claim> claims = [new Claim(ClaimTypes.Name, username), 
            .. roles.Select(role => new Claim(ClaimTypes.Role, role))];

        SecurityTokenDescriptor tokenDescriptor = new() {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public TokenValidationResult ValidateToken(string token) 
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.UTF8.GetBytes(_secretKey);

        try 
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, 
                new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
            string username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            List<string> roles = [.. jwtToken.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)];

            return new TokenValidationResult {
                IsValid = true,
                Username = username,
                Roles = roles
            };
        }
        catch 
        {
            return new TokenValidationResult { IsValid = false };
        }
    }
}