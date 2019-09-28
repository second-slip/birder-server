using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    //***************************
    // ToDo: Add logging
    //***************************
    public class ManageControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUrlService> _urlService;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<ILogger<ManageController>> _logger;

        public ManageControllerTests()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<ManageController>>();
            _urlService = new Mock<IUrlService>();
            _emailSender = new Mock<IEmailSender>();
        }

        #region GetUserProfileAsync unit tests

        [Fact]
        public async Task GetUserProfileAsync_ReturnsNotFound_WhenUserIsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .Returns(Task.FromResult<ApplicationUser>(null));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, mockUserManager.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetUserProfileAsync();

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        //[Fact]
        //public async Task GetUserProfileAsync_ReturnsBadRequest_WhenRepositoryReturnsNull()

        #endregion


    }
}
