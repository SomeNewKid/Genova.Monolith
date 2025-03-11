using Microsoft.AspNetCore.Mvc;
using Genova.ContentService.Protos;
using Genova.SecurityService.Protos;

namespace Genova.WebController.Controllers;

[Route("/")]
public class HomeController : Controller {
    private readonly Content.ContentClient _contentClient;
    private readonly Security.SecurityClient _securityClient;

    public HomeController(Security.SecurityClient securityClient, Content.ContentClient contentClient) {
        _securityClient = securityClient;
        _contentClient = contentClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index() {
        // Simulated JWT (In production, extract this from request headers or cookies)
        string jwtToken = "sample-jwt-token";

        TokenRequest tokenRequest = new() { Token = jwtToken };
        TokenResponse tokenResponse = await _securityClient.ValidateTokenAsync(tokenRequest);

        ContentRequest contentRequest = new() { Culture = "en", UrlPath = "/" };
        ContentResponse contentResponse = await _contentClient.FetchContentAsync(contentRequest);

        string tokenSummary = tokenResponse.IsValid 
            ? "Welcome, " + tokenResponse.Username 
            : "Invalid Token";
        string tokenDetails = tokenResponse.IsValid 
            ? $"Roles: {string.Join(", ", tokenResponse.Roles)}"
            : tokenResponse.ErrorMessage;

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
            <h2>{tokenSummary}</h2>
            <p>{tokenDetails}</p>
            <textarea style="width:80%;height:400px;font-family:monospace">
                {contentResponse.Content}
            </textarea>
        </body>
        </html>
        """;

        return Content(htmlContent, "text/html");
    }
}
