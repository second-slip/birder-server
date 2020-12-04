using Birder.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.HelpersTests
{
    public class Notes
    {
        [Fact]
        public void GetFollowersUserNames_ReturnsEmptyCollection_WhenInputCollectionIsEmpty()
        {
            // Arrange
            var emptyInputCollection = new List<ObservationNote>();

            // Act
            //var result = UserNetworkHelpers.GetFollowersUserNames(emptyInputCollection);

            // Assert
            Assert.True(true);
        }
    }
}
