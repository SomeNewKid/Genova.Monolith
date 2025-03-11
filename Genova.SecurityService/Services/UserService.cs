namespace Genova.SecurityService.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new()
    {
        new User { Username = "admin", Password = "admin123", Roles = new List<string> { "Admin" } },
        new User { Username = "user", Password = "user123", Roles = new List<string> { "User" } }
    };

    public User? ValidateUser(string username, string password)
    {
        return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}
