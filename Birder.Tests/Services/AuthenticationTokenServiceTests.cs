using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Birder.Tests.Controller;

public class AuthenticationTokenServiceTests
{
    private readonly Mock<IConfiguration> _config;

    public AuthenticationTokenServiceTests()
    {
        // _logger = new Mock<ILogger<AuthenticationController>>();
        _config = new Mock<IConfiguration>();
        _config.SetupGet(x => x[It.Is<string>(s => s == "Scheme")]).Returns("https://");
        _config.SetupGet(x => x[It.Is<string>(s => s == "Domain")]).Returns("localhost:99999");
        // _config.SetupGet(x => x[It.Is<string>(s => s == "FlickrApiKey")]).Returns("ggjh");
        // _config.SetupGet(x => x[It.Is<string>(s => s == "MapApiKey")]).Returns("fjfgjn");
        _config.SetupGet(x => x[It.Is<string>(s => s == "TokenKey")]).Returns("fjfgdfdfeTTjn3wq");
    }

    [Fact]
    public void CreateToken_Returns_AuthenticationResultDto_On_Success()
    {
        // Arrange
        var systemClock = new SystemClockService();
        var service = new AuthenticationTokenService(_config.Object, systemClock);

        var expectedClaims = new List<Claim>
        {
            new Claim("ExampleClaim", "Example"),
        };

        var result = service.CreateToken(expectedClaims);
        
        Assert.NotNull(result.AuthenticationToken);
        Assert.NotEmpty(result.AuthenticationToken);
        Assert.Equal(result.FailureReason, AuthenticationFailureReason.None);
    }


    [Theory, MemberData(nameof(NullOrEmpmtyTestData))]
    public void CreateToken_NullOrEmptyClaims_ThrowsArgumentException(List<Claim> claims)
    {
        // Arrange
        var systemClock = new SystemClockService();
        var service = new AuthenticationTokenService(_config.Object, systemClock);

        // Assert
        Assert.Throws<ArgumentException>(
            // Act      
            () => service.CreateToken(claims));

    }

    public static IEnumerable<object[]> NullOrEmpmtyTestData
    {
        get
        {
            return new[]
            {
                    // empty collection
                    new object[] { new List<Claim>() },
                    // no items with id == 0
                    new object[] { null }
                };
        }
    }

}