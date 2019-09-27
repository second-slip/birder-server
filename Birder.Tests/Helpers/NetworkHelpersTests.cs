using Birder.Data.Model;
using Birder.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Birder.Tests.Helpers
{
    public class NetworkHelpersTests
    {
        [Fact]
        // empty list
        public void GetFollowersUserNames_ReturnsEmptyCollection_WhenInputCollectionIsEmpty()
        {
            // Arrange
            var emptyInputCollection = new List<Network>();

            // Act
            var result = NetworkHelpers.GetFollowersUserNames(emptyInputCollection);

            // Assert
            Assert.IsType<List<String>>(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(59)]
        // list.count > 0
        public void GetFollowersUserNames_ReturnsCollection_WhenInputCollectionLengthIsGreaterThan0(int length)
        {
            // Arrange
            var testCollection = GetDynamicNetworkCollection(length);

            // Act
            var result = NetworkHelpers.GetFollowersUserNames(testCollection);

            // Assert
            Assert.IsType<List<String>>(result);
            Assert.NotEmpty(result);
            Assert.Equal(length, result.Count);
            Assert.Equal("Test 1", result[0]);
            Assert.Equal("Test " + length.ToString(), result[length - 1]);
        }

        [Fact]
        public void GetFollowersUserNames_ReturnsNullReferenceException_WhenArgumentIsNull()
        {
            // Arrange
            var emptyInputCollection = new List<Network>();

            // Act & Assert
            var ex = Assert.Throws<NullReferenceException>(() => NetworkHelpers.GetFollowersUserNames(null));
            Assert.Equal("The followers collection is null", ex.Message);
        }

        #region GetFollowingUserNames unit tests

        [Fact]
        // empty list
        public void GetFollowingUserNames_ReturnsEmptyCollection_WhenInputCollectionIsEmpty()
        {
            // Arrange
            var emptyInputCollection = new List<Network>();

            // Act
            var result = NetworkHelpers.GetFollowingUserNames(emptyInputCollection);

            // Assert
            Assert.IsType<List<String>>(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(59)]
        public void GetFollowingUserNames_ReturnsCollection_WhenInputCollectionLengthIsGreaterThan0(int length)
        {
            // Arrange
            var testCollection = GetDynamicNetworkCollection(length);

            // Act
            var result = NetworkHelpers.GetFollowingUserNames(testCollection);

            // Assert
            Assert.IsType<List<String>>(result);
            Assert.NotEmpty(result);
            Assert.Equal(length, result.Count);
            Assert.Equal("Test 1", result[0]);
            Assert.Equal("Test " + length.ToString(), result[length - 1]);
        }

        [Fact]
        public void GetFollowingUserNames_ReturnsNullReferenceException_WhenArgumentIsNull()
        {
            // Arrange
            var emptyInputCollection = new List<Network>();

            // Act & Assert
            var ex = Assert.Throws<NullReferenceException>(() => NetworkHelpers.GetFollowingUserNames(null));
            Assert.Equal("The following collection is null", ex.Message);
        }

        #endregion




        private List<Network> GetDynamicNetworkCollection(int length)
        {
            var list = new List<Network>();

            for (int i = 0; i < length; i++)
            {
                list.Add(new Network()
                {
                    Follower = new ApplicationUser { UserName = "Test " + (i + 1).ToString() },
                    ApplicationUser = new ApplicationUser { UserName = "Test " + (i + 1).ToString() }
                });
            }

            return list;
        }
    }
}
