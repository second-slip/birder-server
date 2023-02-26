using Birder.Data;
using Birder.Data.Model;
using Birder.Helpers;
using Birder.TestsHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.HelpersTests
{
    public class GetUserWithNetworkAsyncTests
    {

        public GetUserWithNetworkAsyncTests() { }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetUserWithNetworkAsync_ReturnsException_WhenArgumentIsNullOrEmpty(string username)
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureClean();
                //context.Database.EnsureCreated();

                var userManager = SharedFunctions.InitialiseUserManager(context);

                // Act & Assert
                var ex = await Assert.ThrowsAsync<ArgumentException>(() => userManager.GetUserWithNetworkAsync(username));
                Assert.Equal("The argument is null or empty (Parameter 'username')", ex.Message);
            }
        }

        [Fact]
        public async Task GetUserWithNetworkAsync_ReturnsUser_WithNetwork()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                string usernameToAct = "User1";
                string usernameToFollow = "User2";

                context.Database.EnsureClean();
                //context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();

                context.Users.Add(SharedFunctions.CreateUser(usernameToAct));
                context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
                context.SaveChanges();
                context.Users.Count().ShouldEqual(2);

                var userManager = SharedFunctions.InitialiseUserManager(context);

                var userToAct = await userManager.FindByNameAsync(usernameToAct);
                var userToFollow = await userManager.FindByNameAsync(usernameToFollow);

                // userToAct follows userToFollow
                context.Network.Add(new Network()
                {
                    ApplicationUser = userToFollow,
                    Follower = userToAct,
                });

                // userToFollow follows userToAct
                context.Network.Add(new Network()
                {
                    ApplicationUser = userToAct,
                    Follower = userToFollow,
                    //FollowerId = userToAct.Id
                });

                context.SaveChanges();
                context.Network.Count().ShouldEqual(2);

                // Act
                var actual = await userManager.GetUserWithNetworkAsync(usernameToAct);

                // Assert
                actual.ShouldBeType<ApplicationUser>();
                actual.Following.Count.ShouldEqual(1);
                actual.Following.FirstOrDefault().ApplicationUser.UserName.ShouldEqual(usernameToFollow);
                actual.Followers.Count.ShouldEqual(1);
                actual.Followers.FirstOrDefault().Follower.UserName.ShouldEqual(usernameToFollow);
            }
        }
    }
}
