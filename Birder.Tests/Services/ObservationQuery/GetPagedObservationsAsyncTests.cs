using AutoMapper;
using TestSupport.EfHelpers;

namespace Birder.Tests.Services;

public class GetPagedObservationsAsyncTests
{
    // private readonly ILoggerFactory _loggerFactory;

    [Fact]
    public async Task GetPagedObservationsAsync_Retrieves_Data____()
    {
                        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // Add a console logger
        });
        var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new BirderMappingProfile()), loggerFactory);
        // var mappingConfig = new MapperConfiguration(cfg =>
        //     {
        //         cfg.AddProfile(new BirderMappingProfile());
        //     });
        var _mapper = mappingConfig.CreateMapper();

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

        var service = new ObservationQueryService(_mapper, context, mockService.Object);

        // Act
        var actual = await service.GetPagedObservationsAsync(x =>
                    x.ApplicationUser.UserName == testUsername, 1, 10);

        // Assert
        actual.ShouldBeType<ObservationsPagedDto>();
        actual.TotalItems.ShouldEqual(2);
        Assert.IsAssignableFrom<IEnumerable<ObservationViewDto>>(actual.Items);
        actual.Items.Count().ShouldEqual(2);
    }

    [Fact]
    public async Task GetPagedObservationsAsync_When_Argument_Is_Null_Returns_Argument_Exception()
    {
                        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // Add a console logger
        });
        var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new BirderMappingProfile()), loggerFactory);
        // var mappingConfig = new MapperConfiguration(cfg =>
        // {
        //     cfg.AddProfile(new BirderMappingProfile());
        // });
        var _mapper = mappingConfig.CreateMapper();

        var mockService = new Mock<IBirdThumbnailPhotoService>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var service = new ObservationQueryService(_mapper, context, mockService.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetPagedObservationsAsync(null, 1, 10));
        Assert.Equal("method argument is null or empty (Parameter 'predicate')", ex.Message);
    }
}