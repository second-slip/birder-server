using System.Security.Claims;
using Birder.MinApiEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Birder.Tests.MinApiEndpoints;

public class ListEndpointTests
{
    [Fact]
    public async Task GetTopObservationsAsync_Returns_Ok_When_Request_Resource_Exists()
    {
        // Arrange
        var username = "Andrew";

        var mock = new Mock<IListService>();
        mock.Setup(obs => obs.GetTopObservationsAsync(username))
            .ReturnsAsync(new List<TopObservationsViewModel>());

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        var result = await ListEndpoints.GetTopObservationsAsync(mock.Object, claimsPrincipal);

        // Assert
        Assert.IsType<Ok<IReadOnlyList<TopObservationsViewModel>>>(result.Result);
    }

    [Fact]
    public async Task GetTopObservationsAsync_Returns_BadRequest_When_Request_Resource_Not_Exists()
    {
        // Arrange
        var username = "Andrew";

        var mock = new Mock<IListService>();
        mock.Setup(obs => obs.GetTopObservationsAsync(username))
            .Returns(Task.FromResult<IReadOnlyList<TopObservationsViewModel>>(null));

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        var result = await ListEndpoints.GetTopObservationsAsync(mock.Object, claimsPrincipal);

        // Assert
        Assert.IsType<BadRequest>(result.Result);
    }

    [Fact]
    public async Task GetTopObservationsWithDateFilterAsync_Returns_Ok_When_Request_Resource_Exists()
    {
        // Arrange
        var username = "Andrew";

        var mock = new Mock<IListService>();
        mock.Setup(obs => obs.GetTopObservationsAsync(username, It.IsAny<int>()))
            .ReturnsAsync(new List<TopObservationsViewModel>());

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        var result = await ListEndpoints.GetTopObservationsWithDateFilterAsync(mock.Object, claimsPrincipal, It.IsAny<int>());

        // Assert
        Assert.IsType<Ok<IReadOnlyList<TopObservationsViewModel>>>(result.Result);
    }

    [Fact]
    public async Task GetTopObservationsWithDateFilterAsync_Returns_BadRequest_When_Request_Resource_Not_Exists()
    {
        // Arrange
        var username = "Andrew";

        var mock = new Mock<IListService>();
        mock.Setup(obs => obs.GetTopObservationsAsync(username, It.IsAny<int>()))
            .Returns(Task.FromResult<IReadOnlyList<TopObservationsViewModel>>(null));

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        var result = await ListEndpoints.GetTopObservationsWithDateFilterAsync(mock.Object, claimsPrincipal, It.IsAny<int>());

        // Assert
        Assert.IsType<BadRequest>(result.Result);
    }

    [Fact]
    public async Task GetLifeListAsync_Returns_Ok_When_Request_Resource_Exists()
    {
        // Arrange
        var username = "Andrew";

        var mock = new Mock<IListService>();
        mock.Setup(obs => obs.GetLifeListAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                 .ReturnsAsync(new List<LifeListViewModel>());

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        var result = await ListEndpoints.GetLifeListAsync(mock.Object, claimsPrincipal);

        // Assert
        Assert.IsType<Ok<IReadOnlyList<LifeListViewModel>>>(result.Result);
    }

        [Fact]
    public async Task GetLifeListAsync_Returns_BadRequest_When_Request_Resource_Not_Exists()
    {
        // Arrange
        var username = "Andrew";

        var mock = new Mock<IListService>();
        mock.Setup(obs => obs.GetLifeListAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                 .Returns(Task.FromResult<IReadOnlyList<LifeListViewModel>>(null));

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        var result = await ListEndpoints.GetLifeListAsync(mock.Object, claimsPrincipal);

        // Assert
        Assert.IsType<BadRequest>(result.Result);
    }
}