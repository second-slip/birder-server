using TestSupport.EfHelpers;

namespace Birder.Tests.Services;

public class GetTopObservationsAsyncTests
{
    [Fact]
    public async Task GetTopObservationsAsync_Retrieves_Data()
    {
        var testUsername = "TestUser1";
        var mockService = new Mock<IBirdThumbnailPhotoService>();
        var clockService = new SystemClockService();

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
        context.Observations.Add(SharedFunctions.GetObservationWithCustomObservationDate(
            context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault(), DateTime.UtcNow.AddYears(-1)));
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ListService(context, clockService);

        // Act
        var actual = await service.GetTopObservationsAsync(testUsername);

        // Assert
        actual.Count().ShouldEqual(1);
        actual.First().Count.ShouldEqual(2); // all records (as no date filter)
    }

    [Fact]
    public async Task GetTopObservationsAsync_Date_Overload_Retrieves_Data()
    {
        var testUsername = "TestUser1";
        var mockService = new Mock<IBirdThumbnailPhotoService>();
        var clockService = new SystemClockService();

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
        context.Observations.Add(SharedFunctions.GetObservationWithCustomObservationDate(
            context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault(), DateTime.UtcNow.AddYears(-1)));
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ListService(context, clockService);

        // Act
        var actual = await service.GetTopObservationsAsync(testUsername, -30);

        // Assert
        actual.Count().ShouldEqual(1);
        actual.First().Count.ShouldEqual(1); // only one observation within last 30 days
    }

    [Fact]
    public async Task GetTopObservationsAsync_Returns_Argument_Exception_When_Usename_Argument_Is_Null_()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        var clockService = new SystemClockService();

        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var service = new ListService(context, clockService);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetTopObservationsAsync(null));
        Assert.Equal("method argument is null or empty (Parameter 'username')", ex.Message);
    }

    [Fact]
    public async Task GetTopObservationsAsync_Date_Overload_Returns_Argument_Exception_When_Usename_Argument_Is_Null_()
    {
        var clockService = new SystemClockService();
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var service = new ListService(context, clockService);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetTopObservationsAsync(null, It.IsAny<int>()));
        Assert.Equal("method argument is null or empty (Parameter 'username')", ex.Message);
    }
}