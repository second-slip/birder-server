using Birder.Services;
using System;
using Xunit;

namespace Birder.Tests.Services.Tests
{
    public class SystemClockTests
    {
        [Fact]
        public void GetNowTest()
        {
            //Arrange
            var service = new SystemClock();
            var expected = DateTime.Now;

            //Act
            var actual = service.GetNow;

            //Assert
            Assert.Equal(expected, actual, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void GetTodayTest()
        {
            //Arrange
            var service = new SystemClock();
            var expected = DateTime.Today;

            //Act
            var actual = service.GetToday;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEndOfTodayTest()
        {
            //Arrange
            var service = new SystemClock();
            var expected = DateTime.Today.Date.AddDays(1).AddTicks(-1);

            //Act
            var actual = service.GetEndOfToday;

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
