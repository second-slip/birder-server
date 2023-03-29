namespace Birder.Tests.Services;

public class ServerlessDatabaseServiceUnitTests
{
    [Fact]
    public async Task Service_Returns_String()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.ChangeTracker.Clear(); //NEW LINE ADDED

        // N.b. ConservationStatus are added in ApplicationDbContext.cs
        context.ConservationStatuses.Count().ShouldEqual(5);

        var repository = new ServerlessDatabaseService(context);

        // Act
        var actual = await repository.GetFirstConservationListStatusAsync();// .GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == "Does Not Exist");

        // Assert
        actual.ShouldBeType<string>();
        actual.ShouldEqual("Red");
    }
}