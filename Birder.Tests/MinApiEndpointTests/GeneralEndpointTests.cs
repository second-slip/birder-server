using Birder.MinApiEndpoints;

namespace Birder.Tests.MinApiEndpoints;

public class GeneralEnpointTests
{
    [Fact]
    public void ServerInfo_Returns_String_Result()
    {
        // Act
        var result = GeneralEndpoints.ServerInfo();

        // Assert
        Assert.IsType<string>(result);
        Assert.Contains("birder-server API", result);
    }
}