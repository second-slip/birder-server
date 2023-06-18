using Microsoft.AspNetCore.Mvc.Testing;

namespace Birder.Integration.Tests; /// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    // [InlineData("/Index")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Theory]
    [InlineData("/")]
    public async Task Get_EndpointsReturnCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetStringAsync(url);

        // Assert
        Assert.Contains("birder-server API", response);
    }
}