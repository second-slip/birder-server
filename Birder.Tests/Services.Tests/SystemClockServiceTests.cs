using Birder.Services;
using System;
using Xunit;

namespace Birder.Tests.Services.Tests
{
    public class SystemClockServiceTests
    {
        private readonly ISystemClockService _systemClockService;

        public SystemClockServiceTests()
        {
            _systemClockService = new SystemClockService();
        }

        [Fact]
        public void GetNowTest()
        {
            //Arrange
            //var service = new SystemClockService();
            var expected = DateTime.Now;

            //Act
            var actual = _systemClockService.GetNow;

            //Assert
            Assert.Equal(expected, actual, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void GetTodayTest()
        {
            //Arrange
            var expected = DateTime.Today;

            //Act
            var actual = _systemClockService.GetToday;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEndOfTodayTest()
        {
            //Arrange
            var expected = DateTime.Today.Date.AddDays(1).AddTicks(-1);

            //Act
            var actual = _systemClockService.GetEndOfToday;

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
