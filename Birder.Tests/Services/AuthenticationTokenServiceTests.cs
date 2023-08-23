using Microsoft.Extensions.Options;

namespace Birder.Tests.Services;

public class AuthenticationTokenServiceTests
{
    IOptions<ConfigOptions> testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
    { BaseUrl = "we", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf" });

    public AuthenticationTokenServiceTests() { }

    [Fact]
    public void CreateToken_Returns_AuthenticationResultDto_On_Success()
    {
        // Arrange
        var mock = new Mock<ISystemClockService>();
        mock.SetupGet(x => x.GetNow).Returns(DateTime.UtcNow);
        var service = new AuthenticationTokenService(testOptions, mock.Object);

        // Act
        var result = service.CreateToken(GetValidTestUser());

        // Assert
        string.IsNullOrEmpty(result).ShouldBeFalse();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.IsType<string>(result);
    }

    private ApplicationUser GetValidTestUser()
    {
        var user = new ApplicationUser()
        {
            Email = "a@b.com",
            EmailConfirmed = true,
            UserName = "Test",
            Avatar = ""
        };

        return user;
    }

    [Fact]
    public void CreateToken_NullOrEmptyClaims_ThrowsArgumentException()
    {
        // Arrange
        var mock = new Mock<ISystemClockService>();
        var service = new AuthenticationTokenService(testOptions, mock.Object);

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(
            // Act      
            () => service.CreateToken(new ApplicationUser()));

        Assert.Equal("error creating claims collection", ex.Message);
    }
}