namespace Genova.SecurityService.Services;

public interface IJwtService
{
    string GenerateToken(string username, List<string> roles);

    TokenValidationResult ValidateToken(string token);
}
