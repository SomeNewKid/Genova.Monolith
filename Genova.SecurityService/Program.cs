using Genova.SecurityService.Services;

namespace Genova.SecurityService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();

        // ✅ Register authentication services
        builder.Services.AddSingleton<IJwtService, JwtService>();
        builder.Services.AddSingleton<IUserService, UserService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<AuthService>();

        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}