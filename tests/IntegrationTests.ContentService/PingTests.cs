using Microsoft.AspNetCore.Mvc.Testing;
using Grpc.Net.Client;
using Genova.ContentService;
using Genova.ContentService.Protos;

namespace IntegrationTests.ContentService;

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
            Content.ContentClient grpcClient = new(channel);

            PingResponse response = await grpcClient.PingAsync(new PingRequest());

            Assert.Equal("ContentService is alive!", response.Message);
        }
    }
}
