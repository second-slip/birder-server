using AutoMapper;
using Birder.Data.Repository;


namespace Birder.Tests.Controller
{
    public class PostFollowUserAsyncTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<NetworkController>> _logger;
        //private readonly UserManager<ApplicationUser> _userManager;

        public PostFollowUserAsyncTests()
        {
            //_userManager = SharedFunctions.InitialiseUserManager();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<NetworkController>>();
        }

        [Fact]
        public async Task PostFollowUserAsync_ReturnsNotFound_WhenRequestingUserIsNullFromRepository()
        {
            // var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            // using (var context = new ApplicationDbContext(options))
            // {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();

            using var context = new ApplicationDbContext(options);
            //You have to create the database
            // context.Database.EnsureClean();
            context.Database.EnsureCreated();
            //context.SeedDatabaseFourBooks();

            //context.ConservationStatuses.Add(new ConservationStatus { ConservationList = "Red", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });

            context.Users.Add(SharedFunctions.CreateUser("testUser1"));
            context.Users.Add(SharedFunctions.CreateUser("testUser2"));

            context.SaveChanges();

            context.Users.Count().ShouldEqual(2);

            // Arrange

            //*******************
            var userManager = SharedFunctions.InitialiseUserManager(context);
            //**********************

            var mockRepo = new Mock<INetworkRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

            string requestingUser = "This requested user does not exist";

            string userToFollow = "This requested user does not exist";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
            };

            // Act
            var result = await controller.PostFollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToFollow));

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Requesting user not found", objectResult.Value);
        }





        #region Follow action tests

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        //{

        //    // Arrange
        //    var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        //    var mockRepo = new Mock<INetworkRepository>();

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, mockUserManager.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
        //    };

        //    //Add model error
        //    controller.ModelState.AddModelError("Test", "This is a test model error");


        //    // Act
        //    var result = await controller.PostFollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel("Test User"));

        //    var modelState = controller.ModelState;
        //    Assert.Equal(1, modelState.ErrorCount);
        //    Assert.True(modelState.ContainsKey("Test"));
        //    Assert.True(modelState["Test"].Errors.Count == 1);
        //    Assert.Equal("This is a test model error", modelState["Test"].Errors[0].ErrorMessage);

        //    // test response
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.True(objectResult is BadRequestObjectResult);
        //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        //    //
        //    var actual = Assert.IsType<string>(objectResult.Value);

        //    //Assert.Contains("This is a test model error", "This is a test model error");
        //    Assert.Equal("Invalid modelstate", actual);
        //}



        [Fact]
        public async Task PostFollowUserAsync_ReturnsNotFound_WhenUserToFollowIsNullFromRepository()
        {
            // var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            // using (var context = new ApplicationDbContext(options))
            // {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();

            using var context = new ApplicationDbContext(options);
            //You have to create the database
            // context.Database.EnsureClean();
            context.Database.EnsureCreated();
            //context.SeedDatabaseFourBooks();

            context.Users.Add(SharedFunctions.CreateUser("testUser1"));
            context.Users.Add(SharedFunctions.CreateUser("testUser2"));

            context.SaveChanges();

            context.Users.Count().ShouldEqual(2);

            // Arrange
            //*******************
            var userManager = SharedFunctions.InitialiseUserManager(context);
            //**********************

            var mockRepo = new Mock<INetworkRepository>();
            //mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<string>()))
            //        .Returns(Task.FromResult<ApplicationUser>(null));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

            string requestingUser = "testUser1";

            string userToFollow = "This requested user does not exist";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
            };

            // Act
            var result = await controller.PostFollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToFollow));

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.IsType<string>(objectResult.Value);
            Assert.Equal("User to follow not found", objectResult.Value);
        }


        [Fact]
        public async Task PostFollowUserAsync_ReturnsBadRequest_FollowerAndToFollowAreEqual()
        {
            // var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            // using (var context = new ApplicationDbContext(options))
            // {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();

            using var context = new ApplicationDbContext(options);
            //You have to create the database
            // context.Database.EnsureClean();
            context.Database.EnsureCreated();
            //context.SeedDatabaseFourBooks();

            context.Users.Add(SharedFunctions.CreateUser("testUser1"));
            context.Users.Add(SharedFunctions.CreateUser("testUser2"));

            context.SaveChanges();

            context.Users.Count().ShouldEqual(2);
            // Arrange
            //*******************
            var userManager = SharedFunctions.InitialiseUserManager(context);
            //**********************
            var mockRepo = new Mock<INetworkRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

            string requestingUser = "testUser1";

            string userToFollow = requestingUser;

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
            };

            // Act
            var result = await controller.PostFollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToFollow));

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var actual = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Trying to follow yourself", actual);

        }

        [Fact]
        public async Task PostFollowUserAsync_Returns_500_On_Internal_Error()
        {
            // Arrange
            UserManager<ApplicationUser> userManager = null; //to cause internal error
            var mockRepo = new Mock<INetworkRepository>();
            mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
                .Verifiable();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CompleteAsync())
                .ThrowsAsync(new InvalidOperationException());

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

            string requestingUser = "testUser1";

            string userToFollow = "testUser2";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
            };

            // Act
            var result = await controller.PostFollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToFollow));

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal($"an unexpected error occurred", objectResult.Value);
        }

        [Fact]
        public async Task PostFollowUserAsync_ReturnsOkObject_WhenRequestIsValid()
        {
            //var options = this.CreateUniqueClassOptions<ApplicationDbContext>();
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();

            using var context = new ApplicationDbContext(options);

            //You have to create the database
            // context.Database.EnsureClean();
            context.Database.EnsureCreated();
            //context.SeedDatabaseFourBooks();

            context.Users.Add(SharedFunctions.CreateUser("testUser1"));
            context.Users.Add(SharedFunctions.CreateUser("testUser2"));

            context.SaveChanges();

            context.Users.Count().ShouldEqual(2);

            // Arrange
            //*******************
            var userManager = SharedFunctions.InitialiseUserManager(context);
            //**********************
            var mockRepo = new Mock<INetworkRepository>();
            mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
                .Verifiable();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.CompleteAsync()).Returns(Task.CompletedTask);

            var controller = new NetworkController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object, userManager);

            string requestingUser = "testUser1";

            string userToFollow = "testUser2";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requestingUser) }
            };

            // Act
            var result = await controller.PostFollowUserAsync(SharedFunctions.GetTestNetworkUserViewModel(userToFollow));

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<NetworkUserViewModel>(objectResult.Value);

            var model = objectResult.Value as NetworkUserViewModel;
            Assert.Equal(userToFollow, model.UserName);
            // }
        }

        #endregion

    }
}
