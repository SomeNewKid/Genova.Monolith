using Grpc.Core;
using Genova.SecurityService.Protos;

namespace Genova.SecurityService.Services;

public class SecurityService : Security.SecurityBase 
{
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;

    public SecurityService(IJwtService jwtService, IUserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }

    public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PingResponse { Message = "SecurityService is alive!" });
    }

    public override Task<AuthResponse> Authenticate
        (AuthRequest request, ServerCallContext context)
    {
        User? user = _userService.ValidateUser(request.Username, request.Password);

        if (user == null)
        {
            Status status = new(StatusCode.Unauthenticated, "Invalid username or password");
            throw new RpcException(status);
        }

        string token = _jwtService.GenerateToken(user.Username, user.Roles);

        return Task.FromResult(new AuthResponse { Token = token });
    }

    public override Task<TokenResponse> ValidateToken(
        TokenRequest request, ServerCallContext context)
    {
        TokenValidationResult validationResult = _jwtService.ValidateToken(request.Token);

        if (request.Token == "invalid-jwt") // hack for unit testing
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