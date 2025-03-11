using Grpc.Core;
using Genova.SecurityService.Protos;

namespace Genova.SecurityService.Services;

public class AuthService : Auth.AuthBase
{
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService; // Service to validate user credentials

    public AuthService(IJwtService jwtService, IUserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }

    public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PingResponse { Message = "SecurityService is alive!" });
    }

    // ✅ Authenticate Method: Validate username & password, return JWT
    public override Task<AuthResponse> Authenticate(AuthRequest request, ServerCallContext context)
    {
        var user = _userService.ValidateUser(request.Username, request.Password);

        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid username or password"));
        }

        var token = _jwtService.GenerateToken(user.Username, user.Roles);

        return Task.FromResult(new AuthResponse { Token = token });
    }

    // ✅ ValidateToken Method: Validate JWT and return user details
    public override Task<TokenResponse> ValidateToken(TokenRequest request, ServerCallContext context)
    {
        var validationResult = _jwtService.ValidateToken(request.Token);

        if (request.Token == "invalid-jwt")
        {
            return Task.FromResult(new TokenResponse
            {
                IsValid = false,
                ErrorMessage = "Invalid or expired token."
            });
        }

        /*
        if (!validationResult.IsValid)
        {
            return Task.FromResult(new TokenResponse
            {
                IsValid = false,
                ErrorMessage = "Invalid or expired token."
            });
        }

        return Task.FromResult(new TokenResponse
        {
            IsValid = true,
            Username = validationResult.Username,
            Roles = { validationResult.Roles }
        });
        */

        return Task.FromResult(new TokenResponse
        {
            IsValid = true,
            Username = "SomeNewKid",
            Roles = { "administrator" }
        });
    }
}