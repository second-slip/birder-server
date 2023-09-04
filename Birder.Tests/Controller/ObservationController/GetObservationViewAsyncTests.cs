using AutoMapper;
using TestSupport.EfHelpers;

namespace Birder.Tests.Services;

public class GetObservationViewAsyncTests
{
    [Fact]
    public async Task GetObservationViewAsync_Returns_GetObservationView_Dto()
    {
        // Arrange
        var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
        var mapper = mappingConfig.CreateMapper();

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

        var service = new ObservationQueryService(mapper, context, mockService.Object);

        // Act
        var actual = await service.GetObservationViewAsync(1);

        // Assert
        actual.ShouldBeType<ObservationViewDto>();

        actual.Username.ShouldEqual("TestUser1");
        actual.Position.ShouldNotBeNull();
        actual.Notes.ShouldNotBeNull();
    }

        [Fact]
    public async Task GetObservationViewAsync_When_Argument_Is_Zero_Returns_Argument_Exception()
    {
        // Arrange
        var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
        var mapper = mappingConfig.CreateMapper();

        var mockService = new Mock<IBirdThumbnailPhotoService>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var service = new ObservationQueryService(mapper, context, mockService.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetObservationViewAsync(0));
        Assert.Equal("method argument is invalid (zero) (Parameter 'id')", ex.Message);
    }
}