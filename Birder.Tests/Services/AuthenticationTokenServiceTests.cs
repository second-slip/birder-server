using Microsoft.Extensions.Configuration;
using Moq;

namespace Birder.Tests.Controller;

public class AuthenticationTokenServiceTests
{

    private readonly Mock<IConfiguration> _config;

    public AuthenticationTokenServiceTests()
    {
        // _logger = new Mock<ILogger<AuthenticationController>>();
        _config = new Mock<IConfiguration>();
        _config.SetupGet(x => x[It.Is<string>(s => s == "Scheme")]).Returns("https://");
        _config.SetupGet(x => x[It.Is<string>(s => s == "Domain")]).Returns("localhost:55722");
        _config.SetupGet(x => x[It.Is<string>(s => s == "FlickrApiKey")]).Returns("ggjh");
        _config.SetupGet(x => x[It.Is<string>(s => s == "MapApiKey")]).Returns("fjfgjn");
        _config.SetupGet(x => x[It.Is<string>(s => s == "TokenKey")]).Returns("fjfgdfdfeTTjn3wq");
    }

    // [Fact]
    // public async Task Returns_OkObjectResult_With_Dto()
    // {
    // }

}