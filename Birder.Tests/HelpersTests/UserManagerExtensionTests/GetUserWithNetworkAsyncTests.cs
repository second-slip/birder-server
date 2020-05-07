using Birder.Data;
using Birder.Helpers;
using Birder.TestsHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.HelpersTests
{
    public class GetUserWithNetworkAsyncTests
    {

        public GetUserWithNetworkAsyncTests()
        {

        }

        [Fact]
        public async System.Threading.Tasks.Task GetUserWithNetworkAsync_WhenArgumentIsEmpty_ReturnsException()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();

                var userManager = SharedFunctions.InitialiseUserManager(context);

                // Act & Assert
                var ex = await Assert.ThrowsAsync<ArgumentException>(() => userManager.GetUserWithNetworkAsync(string.Empty));
                Assert.Equal("The argument is null or empty (Parameter 'username')", ex.Message);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUserWithNetworkAsync_()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                //context.Users.Add(SharedFunctions.CreateUser("testUser1"));
                //context.Users.Add(SharedFunctions.CreateUser("testUser2"));

                //context.SaveChanges();

                //context.Users.Count().ShouldEqual(0);

                // Arrange
                var userManager = SharedFunctions.InitialiseUserManager(context);



                // Act & Assert
                //var ex = Assert.Throws<ArgumentNullException>(() => ObservationsAnalysisHelper.MapLifeList(null));
                var ex = await Assert.ThrowsAsync<ArgumentException>(() => userManager.GetUserWithNetworkAsync(string.Empty));
                Assert.Equal("The argument is null or empty (Parameter 'username')", ex.Message);
                //if (string.IsNullOrEmpty(username))
                //    throw new ArgumentException("The argument is null or empty", nameof(username));
            }

        }


    }
}
