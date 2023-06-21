using TestSupport.EfHelpers;

namespace Birder.Tests.Services;

public class GetTopObservationsAsyncTests
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
        context.Observations.Add(SharedFunctions.GetObservationWithCustomObservationDate(
            context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault(), DateTime.UtcNow.AddYears(-1)));
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ListService(context);

        // Act
        var actual = await service.GetTopObservationsAsync(testUsername, DateTime.UtcNow.AddDays(-30));

        // Assert
        Assert.IsType<TopObservationsAnalysisViewModel>(actual);
        Assert.IsAssignableFrom<List<TopObservationsViewModel>>(actual.TopObservations);
        Assert.IsAssignableFrom<List<TopObservationsViewModel>>(actual.TopMonthlyObservations);
        actual.TopObservations.Count().ShouldEqual(1);
        actual.TopObservations.First().Count.ShouldEqual(2); // two observations of the same species
        actual.TopMonthlyObservations.Count().ShouldEqual(1);
        actual.TopMonthlyObservations.First().Count.ShouldEqual(1); // only one observation within last 30 days
    }

    [Fact]
    public async Task GetTopObservationsAsync_When_Argument_Is_Null_Returns_Argument_Exception()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var service = new ListService(context);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetTopObservationsAsync(null, It.IsAny<DateTime>()));
        Assert.Equal("method argument is null or empty (Parameter 'username')", ex.Message);
    }
}