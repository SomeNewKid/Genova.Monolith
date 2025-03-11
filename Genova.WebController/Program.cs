using Genova.ContentService.Protos;
using Genova.SecurityService.Protos;

namespace Genova.WebController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ✅ Enable MVC Controllers
        builder.Services.AddControllers();

        // ✅ Register gRPC client for Auth service
        builder.Services.AddGrpcClient<Auth.AuthClient>(options =>
        {
            options.Address = new Uri("https://localhost:7142"); // SecurityService gRPC endpoint
        });

        // ✅ Register gRPC client for Content service
        builder.Services.AddGrpcClient<Content.ContentClient>(options =>
        {
            options.Address = new Uri("https://localhost:7288"); // ContentService gRPC endpoint
        });

        var app = builder.Build();

        // ✅ Map controllers
        app.MapControllers();

        app.Run();
    }
}
