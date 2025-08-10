using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Caching.Memory;

namespace Birder.Tests.Controller;

public class PutObservationAsyncTests
{
    private IMemoryCache _cache;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ObservationController>> _logger;
    private readonly ISystemClockService _systemClock;

    // private readonly ILoggerFactory _loggerFactory;

    public PutObservationAsyncTests()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // Add a console logger
        });
        _cache = new MemoryCache(new MemoryCacheOptions());
        _logger = new Mock<ILogger<ObservationController>>();
        // private readonly ILoggerFactory _loggerFactory;
        var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new BirderMappingProfile()), loggerFactory);
        // var mappingConfig = new MapperConfiguration(cfg =>
        // {
        //     cfg.AddProfile(new BirderMappingProfile());
        // });
        _mapper = mappingConfig.CreateMapper();
        _systemClock = new SystemClockService();
    }

    #region PutObservationAsync

    //[Theory]
    //[InlineData(1, 3)]
    //[InlineData(2, 4)]
    //[InlineData(3, 5)]
    //public async Task PutObservationAsync_ReturnsBadRequest_OnInvalidModelState(int id, int birdId)
    //{
    //    //Arrange
    //    var model = GetTestObservationEditViewModel(id, birdId);
    //    var requestingUser = GetUser("Any");

    //    var mockUnitOfWork = new Mock<IUnitOfWork>();
    //    var mockBirdRepo = new Mock<IBirdRepository>();
    //    var mockUserManager = SharedFunctions.InitialiseMockUserManager();
    //    var mockObsRepo = new Mock<IObservationRepository>();
    //    var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
    //    var mockObsNotesRepo = new Mock<IObservationNoteRepository>();
    //    //mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
    //    //           .ThrowsAsync(new InvalidOperationException());

    //    var controller = new ObservationController(
    //        _mapper
    //        , _systemClock
    //        , mockUnitOfWork.Object
    //        , mockBirdRepo.Object
    //        , _logger.Object
    //        , mockUserManager.Object
    //        , mockObsRepo.Object
    //        , mockObsPositionRepo.Object
    //        , mockObsNotesRepo.Object);

    //    controller.ControllerContext = new ControllerContext()
    //    {
    //        HttpContext = new DefaultHttpContext()
    //        { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser.UserName) }
    //    };

    //    controller.ModelState.AddModelError("Test", "This is a test model error");

    //    // Act
    //    var result = await controller.PutObservationAsync(id, model);

    //    // Assert
    //    string expectedMessage = "An error occurred";

    //    var objectResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    //    var actual = Assert.IsType<string>(objectResult.Value);
    //    Assert.Equal(expectedMessage, actual);
    //}

    [Fact]
    public async Task PutObservationAsync_ReturnsBadRequest_OnIdNotEqualModelId()
    {
        //Arrange
        int birdId = 1;
        int id = 1;
        int modelId = 2;
        var model = GetTestObservationEditViewModel(modelId, birdId);
        var requestingUser = GetUser("Any");

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockBirdRepo = new Mock<IBirdRepository>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var mockObsRepo = new Mock<IObservationRepository>(); var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
        var mockObsNotesRepo = new Mock<IObservationNoteRepository>();
        //mockObsRepo.Setup(o => o.GetObservationsAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
        //           .ThrowsAsync(new InvalidOperationException());

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
        var result = await controller.PutObservationAsync(id, model);

        // Assert
        string expectedMessage = "An error occurred (id)";

        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal(expectedMessage, actual);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    public async Task PutObservationAsync_ReturnsNotFound_WhenObservationNotFound(int id, int birdId)
    {
        //Arrange
        var model = GetTestObservationEditViewModel(id, birdId);
        var requestingUser = GetUser("Any");

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockBirdRepo = new Mock<IBirdRepository>();
        //mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
        //    .Returns(Task.FromResult<Bird>(null));
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
        //                .ReturnsAsync(requestingUser);
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
        var result = await controller.PutObservationAsync(id, model);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    public async Task PutObservationAsync_ReturnsBadRequest_WhenObservationNotFound(int id, int birdId)
    {
        //Arrange
        var model = GetTestObservationEditViewModel(id, birdId);
        var requestingUser = GetUser("Any");

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockBirdRepo = new Mock<IBirdRepository>();
        //mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
        //    .Returns(Task.FromResult<Bird>(null));
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
        //                .ReturnsAsync(requestingUser);
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
        var result = await controller.PutObservationAsync(id, model);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    public async Task PutObservationAsync_ReturnsUnauthorised_WhenRequestingUserIsNotObservationOwner(int id, int birdId)
    {
        //Arrange
        var requestingUser = GetUser("Any");
        var model = GetTestObservationEditViewModel(id, birdId, requestingUser);

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockBirdRepo = new Mock<IBirdRepository>();
        //mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
        //    .Returns(Task.FromResult<Bird>(null));
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
        //                .ReturnsAsync(requestingUser);
        var mockObsRepo = new Mock<IObservationRepository>();
        mockObsRepo.Setup(obs => obs.GetObservationAsync(It.IsAny<int>()))
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
        var result = await controller.PutObservationAsync(id, model);

        // Assert

        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    public async Task PutObservationAsync_ReturnsBadRequest_OnException(int id, int birdId)
    {
        //Arrange
        var model = GetTestObservationEditViewModel(id, birdId);
        var requestingUser = GetUser("Any");

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockBirdRepo = new Mock<IBirdRepository>();
        //mockBirdRepo.Setup(b => b.GetBirdAsync(It.IsAny<int>()))
        //    .ReturnsAsync(GetTestBird(birdId));
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        //mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
        //                .ReturnsAsync(requestingUser);
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
        var result = await controller.PutObservationAsync(id, model);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 5)]
    [InlineData(45, 12)]
    // NNED TO ADD NOTES REPO MOCK METHOD .ADD & .REMOVE....
    public async Task PutObservationAsync_ReturnsOkWithObservationViewModel_OnSuccess(int id, int birdId)
    {
        //Arrange
        var model = GetTestObservationEditViewModel(id, birdId);
        var requestingUser = GetUser("Any");

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(w => w.CompleteAsync())
            .Returns(Task.CompletedTask);
        var mockBirdRepo = new Mock<IBirdRepository>();
        mockBirdRepo.Setup(b => b.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(GetTestBird(birdId));
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync(requestingUser);
        var mockObsRepo = new Mock<IObservationRepository>();
        mockObsRepo.Setup(obs => obs.GetObservationAsync(It.IsAny<int>()))
            .ReturnsAsync(GetTestObservation(id, requestingUser));
        var mockObjectValidator = new Mock<IObjectModelValidator>();
        mockObjectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                          It.IsAny<ValidationStateDictionary>(),
                                          It.IsAny<string>(),
                                          It.IsAny<Object>()));
        var mockObsPositionRepo = new Mock<IObservationPositionRepository>();
        mockObsPositionRepo.Setup(p => p.SingleOrDefaultAsync(It.IsAny<Expression<Func<ObservationPosition, bool>>>()))
            .ReturnsAsync(SharedFunctions.GetObservationPosition());
        var mockObsNotesRepo = new Mock<IObservationNoteRepository>();
        mockObsNotesRepo.Setup(on => on.FindAsync(It.IsAny<Expression<Func<ObservationNote, bool>>>()))
            .ReturnsAsync(GetTestObservationNotes(5));

        mockObsNotesRepo.Setup(n => n.RemoveRange(It.IsAny<IEnumerable<ObservationNote>>()))
            .Verifiable();
        mockObsNotesRepo.Setup(n => n.AddRange(It.IsAny<IEnumerable<ObservationNote>>()))
            .Verifiable();

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
        var result = await controller.PutObservationAsync(id, model);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        var expected = new { observationId = id };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    #endregion

    private IEnumerable<ObservationNote> GetTestObservationNotes(int qty)
    {
        var notes = new List<ObservationNote>();
        for (int i = 0; i < qty; i++)
        {
            notes.Add(
            new ObservationNote()
            {
                Id = i + 1,
                NoteType = ObservationNoteType.General,
                Note = "Test note " + i + 1,
            });
        }
        return notes;
    }


    private ObservationUpdateDto GetTestObservationEditViewModel(int id, int birdId)
    {
        return new ObservationUpdateDto()
        {
            ObservationId = id,
            Bird = new BirdSummaryDto() { BirdId = birdId },
            Position = new ObservationPositionDto() { }
        };
    }

    private ObservationUpdateDto GetTestObservationEditViewModel(int id, int birdId, ApplicationUser user)
    {
        return new ObservationUpdateDto()
        {
            ObservationId = id,
            Bird = new BirdSummaryDto() { BirdId = birdId },
            Position = new ObservationPositionDto() { }
        };
    }

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
            ObservationDateTime = _systemClock.GetNow,
            Position = new ObservationPosition() { }
        };
    }

    private Bird GetTestBird(int birdId)
    {
        return new Bird() { BirdId = birdId };
    }
}