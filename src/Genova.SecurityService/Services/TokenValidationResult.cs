namespace Genova.SecurityService.Services;

public class TokenValidationResult
{
    public bool IsValid { get; set; }
    public string Username { get; set; } = "";
    public List<string> Roles { get; set; } = [];
}
