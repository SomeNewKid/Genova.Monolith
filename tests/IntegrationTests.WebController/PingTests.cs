using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Grpc.Core;
using Genova.WebController;
using Genova.SecurityService.Protos;
using Genova.ContentService.Protos;

namespace IntegrationTests.WebController;

public class PingTests : WebControllerTestBase
{
    public PingTests(WebApplicationFactory<Program> factory) : base(factory)
    { }

    [Fact]
    public async Task Ping_should_return_success_message_for_WebController()
    {
        // ✅ No mock setup needed; just check WebController is alive
        var client = CreateClientWithMocks();

        var response = await client.GetAsync("/ping");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("WebController is alive!", content);
    }

    [Fact]
    public async Task Ping_should_return_success_message_for_SecurityService()
    {
        // ✅ Mock SecurityService Ping response
        var mockAuthClient = new Mock<Auth.AuthClient>();
        mockAuthClient
            .Setup(securityClient => securityClient.PingAsync(It.IsAny<Genova.SecurityService.Protos.PingRequest>(), null, null, default))
            .Returns(new AsyncUnaryCall<Genova.SecurityService.Protos.PingResponse>(
                Task.FromResult(new Genova.SecurityService.Protos.PingResponse
                {
                    Message = "SecurityService is alive!"
                }),
                Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { }));

        var client = CreateClientWithMocks(authMock: mockAuthClient);

        var response = await client.GetAsync("/ping");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("SecurityService is alive!", content);
    }

    [Fact]
    public async Task Ping_should_return_success_message_for_ContentService()
    {
        // ✅ Mock ContentService Ping response
        var mockContentClient = new Mock<Content.ContentClient>();
        mockContentClient
            .Setup(contentClient => contentClient.PingAsync(It.IsAny<Genova.ContentService.Protos.PingRequest>(), null, null, default))
            .Returns(new AsyncUnaryCall<Genova.ContentService.Protos.PingResponse>(
                Task.FromResult(new Genova.ContentService.Protos.PingResponse
                {
                    Message = "ContentService is alive!"
                }),
                Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { }));

        var client = CreateClientWithMocks(contentMock: mockContentClient);

        var response = await client.GetAsync("/ping");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("ContentService is alive!", content);
    }

    [Fact]
    public async Task Ping_should_return_error_if_SecurityService_fails()
    {
        // ❌ Mock SecurityService failure response
        var mockAuthClient = new Mock<Auth.AuthClient>();
        mockAuthClient
            .Setup(serviceClient => serviceClient.PingAsync(It.IsAny<Genova.SecurityService.Protos.PingRequest>(), null, null, default))
            .Throws(new RpcException(new Status(StatusCode.Unavailable, "SecurityService is down")));

        var client = CreateClientWithMocks(authMock: mockAuthClient);

        var response = await client.GetAsync("/ping");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("SecurityService error:", content);
    }
}