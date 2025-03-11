using Microsoft.AspNetCore.Mvc;
using Genova.ContentService.Protos;
using Genova.SecurityService.Protos;

namespace Genova.WebController.Controllers;

[Route("/ping")]
public class PingController : Controller
{
    private readonly Content.ContentClient _contentClient;
    private readonly Security.SecurityClient _securityClient;

    public PingController(Security.SecurityClient securityClient, Content.ContentClient contentClient)
    {
        _securityClient = securityClient;
        _contentClient = contentClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string authMessage;
        try
        {
            SecurityService.Protos.PingRequest authRequest = new ();
            SecurityService.Protos.PingResponse authResponse = 
                await _securityClient.PingAsync(authRequest);
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

        string contentMessage;
        try
        {
            ContentService.Protos.PingRequest contentRequest = new ();
            ContentService.Protos.PingResponse contentResponse = 
                await _contentClient.PingAsync(contentRequest);
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
