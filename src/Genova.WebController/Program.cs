using Genova.ContentService.Protos;
using Genova.SecurityService.Protos;

namespace Genova.WebController;

public class Program 
{
    public static void Main(string[] args) 
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection services = builder.Services;

        _ = services.AddControllers();

        _ = services.AddGrpcClient<Security.SecurityClient>(options =>
            options.Address = new Uri("https://localhost:7142")
        );

        _ = services.AddGrpcClient<Content.ContentClient>(options =>
            options.Address = new Uri("https://localhost:7288")
        );

        WebApplication app = builder.Build();

        _ = app.MapControllers();

        app.Run();
    }
}