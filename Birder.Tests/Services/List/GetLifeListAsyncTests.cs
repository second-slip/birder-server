using TestSupport.EfHelpers;

namespace Birder.Tests.Services;

public class GetLifeListAsyncTests
{
    [Fact]
    public async Task GetLifeList_Retrieves_Data____()
    {
        var testUsername = "TestUser1";
        var mockService = new Mock<IBirdThumbnailPhotoService>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser(testUsername));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        context.Observations.Add(SharedFunctions.GetObservation(context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault()));
        context.Observations.Add(SharedFunctions.GetObservation(context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault()));
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ListService(context);

        // Act
        var actual = await service.GetLifeListAsync(x => x.ApplicationUser.UserName == testUsername);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<LifeListViewModel>>(actual);
        actual.Count().ShouldEqual(1); // one species
        actual.First().Count.ShouldEqual(2); // two observations of the same species
    }

    [Fact]
    public async Task GetLifeListAsync_When_Argument_Is_Null_Returns_Argument_Exception()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var service = new ListService(context);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetLifeListAsync(null));
        Assert.Equal("method argument is null or empty (Parameter 'predicate')", ex.Message);
    }
}