using Microsoft.Extensions.Options;

namespace Birder.Tests.Services;

public class AuthenticationTokenServiceTests
{
    IOptions<AuthConfigOptions> testOptions = Options.Create<AuthConfigOptions>(new AuthConfigOptions()
    { BaseUrl = "we", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf" });

    public AuthenticationTokenServiceTests() { }

    [Fact]
    public void CreateToken_Returns_AuthenticationResultDto_On_Success()
    {
        // Arrange
        var service = new AuthenticationTokenService(testOptions);

        var result = service.CreateToken(GetValidTestUser());

        string.IsNullOrEmpty(result).ShouldBeFalse();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.IsType<String>(result);
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
        var systemClock = new SystemClockService();
        var service = new AuthenticationTokenService(testOptions);

        // Assert
        var ex = Assert.Throws<InvalidOperationException>(
            // Act      
            () => service.CreateToken(new ApplicationUser()));

        Assert.Equal("error creating claims collection", ex.Message);
    }
}