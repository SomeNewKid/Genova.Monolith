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
        HttpClient client = _factory.CreateDefaultClient();
        using (GrpcChannel channel = GrpcChannel.ForAddress(
            client.BaseAddress!,
            new GrpcChannelOptions { HttpClient = client })) 
        {
            Security.SecurityClient grpcClient = new(channel);

            PingResponse response = await grpcClient.PingAsync(new PingRequest());

            Assert.Equal("SecurityService is alive!", response.Message);
        }
    }
}