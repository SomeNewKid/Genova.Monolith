namespace Genova.SecurityService.Services;

public class UserService : IUserService
{
    private readonly List<User> _users =
    [
        new User { Username = "admin", Password = "admin123", Roles = ["Admin"] },
        new User { Username = "user", Password = "user123", Roles = ["User"] }
    ];

    public User? ValidateUser(string username, string password)
    {
        return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}
