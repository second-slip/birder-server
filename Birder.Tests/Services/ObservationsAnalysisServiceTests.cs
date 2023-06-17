using TestSupport.EfHelpers;

namespace Birder.Tests.Services;

public class ObservationsAnalysisServiceTests
{
    public ObservationsAnalysisServiceTests() { }

    [Fact]
    public async Task GetObservationsSummaryAsync_ReturnsViewModel_WithOneMatchInDb()
    {
        var testUsername = "TestUser1";

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
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);

        var service = new ObservationsAnalysisService(context);

        // Act or change
        var actual = await service.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == testUsername);

        // Assert
        actual.ShouldBeType<ObservationAnalysisViewModel>();
        actual.TotalObservationsCount.ShouldEqual(1);
        actual.UniqueSpeciesCount.ShouldEqual(1);
    }

    [Fact]
    public async Task GetObservationsSummaryAsync_ReturnsEmptyViewModel_WithNoMatchesInDb()
    {
        var testUsername = "TestUser1";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser(testUsername));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);
        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);
        context.Observations.Count().ShouldEqual(0);


        var service = new ObservationsAnalysisService(context);

        // Act or change
        var actual = await service.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == testUsername);

        // Assert
        actual.ShouldBeType<ObservationAnalysisViewModel>();
        actual.TotalObservationsCount.ShouldEqual(0);
        actual.UniqueSpeciesCount.ShouldEqual(0);
    }

    [Fact]
    public async Task GetObservationsSummaryAsync_ReturnsEmptyViewModel_WhenUserDoesNotExist()
    {
        var testUsername = "TestUser1";

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser(testUsername));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);
        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);
        context.Observations.Count().ShouldEqual(0);

        var service = new ObservationsAnalysisService(context);

        // Act or change
        var actual = await service.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == "Does Not Exist");

        // Assert
        actual.ShouldBeType<ObservationAnalysisViewModel>();
        actual.TotalObservationsCount.ShouldEqual(0);
        actual.UniqueSpeciesCount.ShouldEqual(0);

    }

    [Fact]
    public async Task GetObservationsSummaryAsync_ReturnsException_WhenArgumentIsNull()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var service = new ObservationsAnalysisService(context);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetObservationsSummaryAsync(null));
        Assert.Equal("method argument is null or empty (Parameter 'predicate')", ex.Message);
    }
}