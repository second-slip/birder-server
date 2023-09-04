using AutoMapper;
using TestSupport.EfHelpers;

namespace Birder.Tests.Services;

public class GetPagedObservationsFeedAsyncTests
{
    [Fact]
    public async Task GetPagedObservationsFeedAsync________________ReturnsViewModel_WithOneMatchInDb()
    {
        var mappingConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BirderMappingProfile());
        });
        var _mapper = mappingConfig.CreateMapper();

        
        var testUsername = "TestUser1";
        Mock<ILogger<BirdThumbnailPhotoService>> loggerMock = new();
        Mock<FlickrService> j = new();
        var mockService = new Mock<IBirdThumbnailPhotoService>();
        mockService.Setup(f => f.GetThumbnailUrl(It.IsAny<IEnumerable<ObservationFeedDto>>()))
            .ReturnsAsync((IEnumerable<ObservationFeedDto> m) => m);
        // Note: the output object is the same object as the input

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
        // context.Observations.Count().ShouldEqual(1);
        context.Observations.Add(SharedFunctions.GetObservation(context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault()));
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ObservationQueryService(_mapper, context, mockService.Object);

        // Act
        var actual = await service.GetPagedObservationsFeedAsync(x =>
                    x.ApplicationUser.UserName == testUsername, 1, 10);

        // Assert
        Assert.Equal(2, actual.Count());
        Assert.IsAssignableFrom<IEnumerable<ObservationFeedDto>>(actual);
    }

    [Fact]
    public async Task GetPagedObservationsFeedAsync_When_Argument_Is_Null_Returns_Argument_Exception()
    {
                var mappingConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BirderMappingProfile());
        });
        var _mapper = mappingConfig.CreateMapper();

        var mockService = new Mock<IBirdThumbnailPhotoService>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var service = new ObservationQueryService(_mapper, context, mockService.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetPagedObservationsFeedAsync(null, 1, 10));
        Assert.Equal("method argument is null or empty (Parameter 'predicate')", ex.Message);
    }

}