using Genova.SecurityService.Services;

namespace Genova.SecurityService;

public class Program
{
    public static void Main(string[] args) 
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection services = builder.Services;

        _ = services.AddGrpc();

        _ = services.AddSingleton<IJwtService, JwtService>();
        _ = services.AddSingleton<IUserService, UserService>();

        WebApplication app = builder.Build();

        _ = app.MapGrpcService<Services.SecurityService>();

        _ = app.MapGet("/", () =>
            "Communication with gRPC endpoints must be made through a gRPC client. " +
            "To learn how to create a client, visit: " +
            "https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}