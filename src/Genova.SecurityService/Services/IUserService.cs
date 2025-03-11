namespace Genova.SecurityService.Services;

public interface IUserService
{
    User? ValidateUser(string username, string password);
}