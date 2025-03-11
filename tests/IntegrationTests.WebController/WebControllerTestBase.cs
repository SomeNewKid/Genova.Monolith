using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Genova.WebController;
using Genova.SecurityService.Protos;
using Genova.ContentService.Protos;

namespace IntegrationTests.WebController;

public class WebControllerTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WebControllerTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    protected HttpClient CreateClientWithMocks(
        Mock<Auth.AuthClient>? authMock = null,
        Mock<Content.ContentClient>? contentMock = null)
    {
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                if (authMock != null)
                {
                    services.AddSingleton(authMock.Object);
                }
                if (contentMock != null)
                {
                    services.AddSingleton(contentMock.Object);
                }
            });
        }).CreateClient();
    }
}
