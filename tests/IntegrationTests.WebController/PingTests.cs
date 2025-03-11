using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Grpc.Core;
using Genova.WebController;
using Genova.SecurityService.Protos;
using Genova.ContentService.Protos;

namespace IntegrationTests.WebController;

public class PingTests : WebControllerTestBase {

    public PingTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Ping_should_return_success_message_for_WebController() 
    {
        HttpClient client = CreateClientWithMocks();

        HttpResponseMessage response = await client.GetAsync("/ping");
        _ = response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        Assert.Contains("WebController is alive!", content);
    }

    [Fact]
    public async Task Ping_should_return_success_message_for_SecurityService() {
        Mock<Security.SecurityClient> mockAuthClient = new();
        _ = mockAuthClient
            .Setup(securityClient => securityClient.PingAsync(
                It.IsAny<Genova.SecurityService.Protos.PingRequest>(), null, null, default))
            .Returns(new AsyncUnaryCall<Genova.SecurityService.Protos.PingResponse>(
                Task.FromResult(new Genova.SecurityService.Protos.PingResponse {
                    Message = "SecurityService is alive!"
                }),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => [],
                () => { }));

        HttpClient client = CreateClientWithMocks(authMock: mockAuthClient);

        HttpResponseMessage response = await client.GetAsync("/ping");
        _ = response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        Assert.Contains("SecurityService is alive!", content);
    }

    [Fact]
    public async Task Ping_should_return_success_message_for_ContentService() {
        Mock<Content.ContentClient> mockContentClient = new();
        _ = mockContentClient
            .Setup(contentClient => contentClient.PingAsync(
                It.IsAny<Genova.ContentService.Protos.PingRequest>(), null, null, default))
            .Returns(new AsyncUnaryCall<Genova.ContentService.Protos.PingResponse>(
                Task.FromResult(new Genova.ContentService.Protos.PingResponse {
                    Message = "ContentService is alive!"
                }),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => [],
                () => { }));

        HttpClient client = CreateClientWithMocks(contentMock: mockContentClient);

        HttpResponseMessage response = await client.GetAsync("/ping");
        _ = response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        Assert.Contains("ContentService is alive!", content);
    }

    [Fact]
    public async Task Ping_should_return_error_if_SecurityService_fails() {
        Mock<Security.SecurityClient> mockAuthClient = new();
        _ = mockAuthClient
            .Setup(serviceClient => serviceClient.PingAsync(
                It.IsAny<Genova.SecurityService.Protos.PingRequest>(), null, null, default))
            .Throws(new RpcException(new Status(
                StatusCode.Unavailable, "SecurityService is down")));

        HttpClient client = CreateClientWithMocks(authMock: mockAuthClient);

        HttpResponseMessage response = await client.GetAsync("/ping");
        _ = response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        Assert.Contains("SecurityService error:", content);
    }
}
