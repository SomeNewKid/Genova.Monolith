using Microsoft.AspNetCore.Mvc;
using Genova.ContentService.Protos;
using Genova.SecurityService.Protos;

namespace Genova.WebController.Controllers;

[Route("/ping")]
public class PingController : Controller
{
    private readonly Content.ContentClient _contentClient;
    private readonly Auth.AuthClient _authClient;

    public PingController(Auth.AuthClient authClient, Content.ContentClient contentClient)
    {
        _authClient = authClient;
        _contentClient = contentClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var authMessage = "";
        try
        {
            var authRequest = new SecurityService.Protos.PingRequest();
            var authResponse = await _authClient.PingAsync(authRequest);
            authMessage = authResponse.Message;
            if (string.IsNullOrWhiteSpace(authMessage))
            {
                authMessage = "SecurityService is not responding!";
            }
        }
        catch (Exception exception)
        {
            authMessage = $"SecurityService error: {exception.Message}";
        }

        var contentMessage = "";
        try
        {
            var contentRequest = new ContentService.Protos.PingRequest();
            var contentResponse = await _contentClient.PingAsync(contentRequest);
            contentMessage = contentResponse.Message;
            if (string.IsNullOrWhiteSpace(contentMessage))
            {
                contentMessage = "ContentService is not responding!";
            }
        }
        catch (Exception exception)
        {
            contentMessage = $"ContentService error: {exception.Message}";
        }

        // ✅ Build HTML response dynamically
        string htmlContent = $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Ping!</title>
        </head>
        <body>
            <h1>Pings</h1>
            <p>WebController is alive!</p>
            <p>{authMessage}</p>
            <p>{contentMessage}</p>
        </body>
        </html>
        """;

        return Content(htmlContent, "text/html");
    }
}
