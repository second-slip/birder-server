using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Birder.Tests.Helpers
{
    public class UserProfileHelperTests
    {
        [Fact]
        public void GetFollowersUserNames_ReturnsEmptyCollection_WhenInputCollectionIsEmpty()
        {
            // Arrange
            var emptyInputCollection = new List<Foll>();

            // Act
            var result = UserProfileHelper.GetFollowersUserNames(emptyInputCollection);

            // Assert
            Assert.IsType<List<String>>(result);
            Assert.Empty(result);
        }
    }
}
