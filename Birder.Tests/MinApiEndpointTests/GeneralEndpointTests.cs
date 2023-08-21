using Birder.MinApiEndpoints;

namespace Birder.Tests.MinApiEndpoints;

public class GeneralEnpointTests
{
    [Fact]
    public void ServerInfo_Returns_String_Result()
    {
        // Arrange 
        var _systemClock = new SystemClockService();

        // Act
        var result = GeneralEndpoints.ServerInfo(_systemClock);

        // Assert
        Assert.IsType<string>(result);
        Assert.Contains("birder-server API", result);
    }
}