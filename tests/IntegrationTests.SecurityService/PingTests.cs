using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Grpc.Net.Client;
using Genova.SecurityService;
using Genova.SecurityService.Protos;

namespace IntegrationTests.SecurityService;

public class PingTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PingTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Ping_should_return_success_message()
    {
        // ✅ Start in-memory test server
        using var client = _factory.CreateDefaultClient();
        using var channel = GrpcChannel.ForAddress(client.BaseAddress!, new GrpcChannelOptions { HttpClient = client });
        var grpcClient = new Auth.AuthClient(channel);

        // ✅ Call Ping
        var response = await grpcClient.PingAsync(new PingRequest());

        // ✅ Validate response
        Assert.Equal("SecurityService is alive!", response.Message);
    }
}