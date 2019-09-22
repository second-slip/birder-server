using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Repository;
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

        [Fact]
        public async Task GetUser_ReturnsOkObjectResult_WithObject()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            // ---> GetUserAndNetworkAsync
            //mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(DateTime.Today))
            //     .ReturnsAsync(GetTestTweet()); //--> needs a real SystemClockService
            //mockRepo.Setup(repo => repo.GetTweetOfTheDayAsync(It.IsAny<DateTime>()))
            //    .ReturnsAsync(GetTestTweetDay());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            // Act
            var result = await controller.GetUser("");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }


    }
}
