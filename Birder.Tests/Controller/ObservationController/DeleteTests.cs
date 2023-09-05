using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Birder.Tests.Controller;

public class ObservationControllerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ObservationController>> _logger;
    private readonly ISystemClockService _systemClock;

    public ObservationControllerTests()
    {
        _logger = new Mock<ILogger<ObservationController>>();
        var mappingConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BirderMappingProfile());
        });
        _mapper = mappingConfig.CreateMapper();
        _systemClock = new SystemClockService();
    }


    #region DeleteObservationAsync

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteObservationAsync_ReturnsBadRequest_WhenObservationNotFound(int id)
    {
        //Arrange
        var requestingUser = GetUser("Any");
        //var model = GetTestObservationViewModel(id, birdId, requestingUser);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockBirdRepo = new Mock<IBirdRepository>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockObsRepo = new Mock<IObservationRepository>();
        mockObsRepo.Setup(obs => obs.GetObservationAsync(It.IsAny<int>()))
            .Returns(Task.FromResult<Observation>(null));
        var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
        var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

        var controller = new ObservationController(
            _mapper
            , _systemClock
            , mockUnitOfWork.Object
            , mockBirdRepo.Object
            , _logger.Object
            , mockUserManager.Object
            , mockObsRepo.Object
            , mockObsPositionRepo.Object
            , mockObsNotesRepo.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
        };

        // Act
        var result = await controller.DeleteObservationAsync(id);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    public async Task DeleteObservationAsync_ReturnsUnauthorised_WhenRequestingUserIsNotObservationOwner(int id, int birdId)
    {
        //Arrange
        var requestingUser = GetUser("Any");
        var model = GetTestObservation(id, requestingUser);//, birdId);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockBirdRepo = new Mock<IBirdRepository>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockObsRepo = new Mock<IObservationRepository>();
        mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>()))
            .ReturnsAsync(GetTestObservation(0, new ApplicationUser { UserName = "Someone else" }));
        var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
        var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

        var controller = new ObservationController(
            _mapper
            , _systemClock
            , mockUnitOfWork.Object
            , mockBirdRepo.Object
            , _logger.Object
            , mockUserManager.Object
            , mockObsRepo.Object
            , mockObsPositionRepo.Object
            , mockObsNotesRepo.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
        };

        // Act
        var result = await controller.DeleteObservationAsync(id);

        // Assert

        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteObservationAsync_ReturnsBadRequest_OnException(int id)
    {
        //Arrange
        var requestingUser = GetUser("Any");
        var model = GetTestObservation(id, requestingUser);//, birdId);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockBirdRepo = new Mock<IBirdRepository>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockObsRepo = new Mock<IObservationRepository>();
        mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>()))
                   .Throws(new InvalidOperationException());
        var mockObjectValidator = new Mock<IObjectModelValidator>();
        mockObjectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                          It.IsAny<ValidationStateDictionary>(),
                                          It.IsAny<string>(),
                                          It.IsAny<Object>()));
        var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
        var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

        var controller = new ObservationController(
            _mapper
            , _systemClock
            , mockUnitOfWork.Object
            , mockBirdRepo.Object
            , _logger.Object
            , mockUserManager.Object
            , mockObsRepo.Object
            , mockObsPositionRepo.Object
            , mockObsNotesRepo.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
        };

        controller.ObjectValidator = mockObjectValidator.Object;

        // Act
        var result = await controller.DeleteObservationAsync(id);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(45)]
    public async Task DeleteObservationAsync_ReturnsOk_OnSuccess(int id)
    {
        //Arrange
        var requestingUser = GetUser("Any");

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(w => w.CompleteAsync())
            .Returns(Task.CompletedTask);
        var mockBirdRepo = new Mock<IBirdRepository>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockObsRepo = new Mock<IObservationRepository>();
        mockObsRepo.Setup(o => o.GetObservationAsync(It.IsAny<int>()))
            .ReturnsAsync(GetTestObservation(id, requestingUser));
        var mockObjectValidator = new Mock<IObjectModelValidator>();
        mockObjectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                          It.IsAny<ValidationStateDictionary>(),
                                          It.IsAny<string>(),
                                          It.IsAny<Object>()));
        var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
        var mockObsNotesRepo = new Mock<IObservationNoteRepository>();

        var controller = new ObservationController(
            _mapper
            , _systemClock
            , mockUnitOfWork.Object
            , mockBirdRepo.Object
            , _logger.Object
            , mockUserManager.Object
            , mockObsRepo.Object
            , mockObsPositionRepo.Object
            , mockObsNotesRepo.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
        };

        controller.ObjectValidator = mockObjectValidator.Object;

        // Act
        var result = await controller.DeleteObservationAsync(id);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { observationId = id };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    #endregion


    private ApplicationUser GetUser(string username)
    {
        return new ApplicationUser()
        {
            UserName = username
        };
    }

    private Observation GetTestObservation(int id, ApplicationUser user)
    {
        return new Observation()
        {
            ObservationId = id,
            ApplicationUser = user,
            ObservationDateTime = _systemClock.GetNow
        };
    }
}