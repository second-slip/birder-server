using Birder.Data.Model;
using Birder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Birder.Tests.Helpers
{
    public class NetworkHelpersTests
    {
        [Fact]
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
            // Act & Assert
            var ex = Assert.Throws<NullReferenceException>(() => NetworkHelpers.GetFollowersUserNames(null));
            Assert.Equal("The followers collection is null", ex.Message);
        }

        #region GetFollowingUserNames unit tests

        [Fact]
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
            // Act & Assert
            var ex = Assert.Throws<NullReferenceException>(() => NetworkHelpers.GetFollowingUserNames(null));
            Assert.Equal("The following collection is null", ex.Message);
        }

        #endregion

        #region GetFollowersNotBeingFollowedUserNames unit tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(59)]
        public void GetFollowersNotBeingFollowedUserNames_ReturnsEmptyCollection_WhenInputCollectionIsEmpty(int length)
        {
            // Arrange
            var user = new ApplicationUser() { UserName = "Test User" };
            user.Following = GetDynamicNetworkCollection(length);
            user.Followers = new List<Network>();

            // Act
            var result = NetworkHelpers.GetFollowersNotBeingFollowedUserNames(user);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<String>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetFollowersNotBeingFollowedUserNames_ReturnsCollection_WhenCollectionsAreGreaterThanOne()
        {
            // Arrange
            var user = new ApplicationUser() { UserName = "Test User" };
            user.Following = GetDynamicNetworkCollection(3);
            user.Followers = GetDynamicNetworkCollection(6);

            // Act
            var result = NetworkHelpers.GetFollowersNotBeingFollowedUserNames(user);

            // Assert
            var t = Assert.IsAssignableFrom<IEnumerable<String>>(result);
            Assert.Equal(3, t.Count());
        }

        [Fact]
        public void GetFollowersNotBeingFollowedUserNames_ReturnsNullReferenceException_WhenUserIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<NullReferenceException>(() => NetworkHelpers.GetFollowersNotBeingFollowedUserNames(null));
            Assert.Equal("The user is null", ex.Message);
        }

        [Fact]
        public void GetFollowersNotBeingFollowedUserNames_ReturnsNullReferenceException_WhenFollowersCollectionIsNull()
        {
            // Arrange
            var user = new ApplicationUser() { UserName = "Test User" };
            user.Following = new List<Network>();
            user.Followers = null;

            // Act & Assert
            var ex = Assert.Throws<NullReferenceException>(() => NetworkHelpers.GetFollowersNotBeingFollowedUserNames(user));
            Assert.Equal("The followers collection is null", ex.Message);
        }

        [Fact]
        public void GetFollowersNotBeingFollowedUserNames_ReturnsNullReferenceException_WhenFollowingCollectionIsNull()
        {
            // Arrange
            var user = new ApplicationUser() { UserName = "Test User" };
            user.Following = null;
            user.Followers = new List<Network>();

            // Act & Assert
            var ex = Assert.Throws<NullReferenceException>(() => NetworkHelpers.GetFollowersNotBeingFollowedUserNames(user));
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
