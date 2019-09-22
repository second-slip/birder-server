using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class UserControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UserController>> _logger;
        // private readonly IUnitOfWork _unitOfWork;
        // private readonly IUserRepository _userRepository;


        public UserControllerTests()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<UserController>>();

        }

        #region GetUser action tests

        [Fact]
        public async Task GetUser_ReturnsOkObjectResult_WithObject()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOtherUser());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetUser(It.IsAny<string>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUser_ReturnsBadRequest_WhenRepositoryReturnsNull()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .Returns(Task.FromResult<ApplicationUser>(null));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetUser(It.IsAny<string>());

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion


        #region Mock methods

        private ApplicationUser GetLoggedInUser()
        {
            var user = new ApplicationUser()
            {
                UserName = "LoggedInUser Test"
            };


            return user;
        }

        private ApplicationUser GetOtherUser()
        {
            var user = new ApplicationUser()
            {
                UserName = "Other Test"
            };


            return user;
        }

        //ToDo: move to shared
        private ClaimsPrincipal GetTestClaimsPrincipal()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            return user;
        }

        #endregion
    }
}
