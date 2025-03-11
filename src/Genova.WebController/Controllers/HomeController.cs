using Microsoft.AspNetCore.Mvc;
using Genova.ContentService.Protos;
using Genova.SecurityService.Protos;

namespace Genova.WebController.Controllers;

[Route("/")]
public class HomeController : Controller
{
    private readonly Content.ContentClient _contentClient;
    private readonly Auth.AuthClient _authClient;

    public HomeController(Auth.AuthClient authClient, Content.ContentClient contentClient)
    {
        _authClient = authClient;
        _contentClient = contentClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Simulated JWT (In production, extract this from request headers or cookies)
        string jwtToken = "sample-jwt-token";

        // ✅ Call SecurityService gRPC method ValidateToken
        var tokenRequest = new TokenRequest { Token = jwtToken };
        var tokenResponse = await _authClient.ValidateTokenAsync(tokenRequest);

        var contentRequest = new ContentRequest { Culture = "en", UrlPath = "/" };
        var contentResponse = await _contentClient.FetchContentAsync(contentRequest);

        // ✅ Build HTML response dynamically
        string htmlContent = $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Test Page</title>
        </head>
        <body>
            <h1>Test Page</h1>
            <h2>{(tokenResponse.IsValid ? "Welcome, " + tokenResponse.Username : "Invalid Token")}</h2>
            <p>{(tokenResponse.IsValid ? $"Roles: {string.Join(", ", tokenResponse.Roles)}" : tokenResponse.ErrorMessage)}</p>
            <textarea style="width:80%;height:400px;font-family:monospace">{contentResponse.Content}</textarea>
        </body>
        </html>
        """;

        return Content(htmlContent, "text/html");
    }
}
