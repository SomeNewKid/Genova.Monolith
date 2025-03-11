using Genova.ContentService.Services;

namespace Genova.ContentService;

public class Program
{
    public static void Main(string[] args) 
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection services = builder.Services;

        _ = services.AddGrpc();

        WebApplication app = builder.Build();

        _ = app.MapGrpcService<Services.ContentService>();

        _ = app.MapGet("/", () =>
            "Communication with gRPC endpoints must be made through a gRPC client. " +
            "To learn how to create a client, visit: " +
            "https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}