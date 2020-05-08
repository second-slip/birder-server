using Birder.Data;
using Birder.Data.Model;
using Birder.Helpers;
using Birder.TestsHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.HelpersTests
{
    public class GetUsersAsyncTests
    {
        public GetUsersAsyncTests() { }


        #region test with GetFollowersNotFollowed predicate

        //user => followersNotBeingFollowed.Contains(user.UserName

        [Fact]
        public async Task GetUsersAsync_FollowersNotFollowedEmptyCollectionArgument_ReturnsNoUsers()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                string requestingUsername = "TestUser1";
                string usernameToFollow = "TestUser2";

                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();  // int number of users?

                context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
                context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
                context.SaveChanges();
                context.Users.Count().ShouldEqual(2);
                IEnumerable<string> followersNotBeingFollowed = new List<string>();

                var userManager = SharedFunctions.InitialiseUserManager(context);

                // Act
                var actual = await userManager.GetUsersAsync(user => followersNotBeingFollowed.Contains(user.UserName));

                // Assert
                actual.ShouldBeType<List<ApplicationUser>>();
                actual.ShouldBeEmpty();
            }
        }

        [Fact]
        public async Task GetUsersAsync_OneFollowerNotFollowed_ReturnsOneUser()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                string requestingUsername = "TestUser1";
                string followerUsername = "TestUser2";

                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();  // int number of users?

                context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
                context.Users.Add(SharedFunctions.CreateUser(followerUsername));
                context.SaveChanges();
                context.Users.Count().ShouldEqual(2);

                var userManager = SharedFunctions.InitialiseUserManager(context);

                var requestingUser = await userManager.FindByNameAsync(requestingUsername);
                var follower = await userManager.FindByNameAsync(followerUsername);

                // follower follows userToTest
                context.Network.Add(new Network()
                {
                    ApplicationUser = requestingUser,
                    Follower = follower,
                });

                context.SaveChanges();
                context.Network.Count().ShouldEqual(1);

                IEnumerable<string> followersNotBeingFollowed = new List<string> { followerUsername };

                // Act
                var actual = await userManager.GetUsersAsync(user => followersNotBeingFollowed.Contains(user.UserName));

                // Assert
                actual.ShouldBeType<List<ApplicationUser>>();
                actual.Count().ShouldEqual(1);
                actual.FirstOrDefault().UserName.ShouldEqual(followerUsername);
            }
        }

        #endregion


        #region test with SuggestedBirdersToFollow predicate

        //user => !followingUsernamesList.Contains(user.UserName) 
                                            // && user.UserName != requestingUser.UserName

        [Fact]
        public async Task GetUsersAsync_OneSuggestedUserToFollow_ReturnsUser()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                string requestingUsername = "TestUser1";
                string usernameToFollow = "TestUser2";

                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();  // int number of users?

                context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
                context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
                context.SaveChanges();
                context.Users.Count().ShouldEqual(2);
                IEnumerable<string> followingUsernamesList = new List<string>();

                var userManager = SharedFunctions.InitialiseUserManager(context);

                // Act
                var actual = await userManager.GetUsersAsync(user => !followingUsernamesList.Contains(user.UserName) && user.UserName != requestingUsername);

                // Assert
                actual.ShouldBeType<List<ApplicationUser>>();
                actual.Count().ShouldEqual(1);
            }
        }

        [Fact]
        public async Task GetUsersAsync_NoSuggestedUserToFollow_ReturnsNoUser()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                string requestingUsername = "TestUser1";
                string usernameToFollow = "TestUser2";

                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();  // int number of users?

                context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
                context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
                context.SaveChanges();
                context.Users.Count().ShouldEqual(2);
                IEnumerable<string> followingUsernamesList = new List<string> { usernameToFollow };

                var userManager = SharedFunctions.InitialiseUserManager(context);

                // Act
                var actual = await userManager.GetUsersAsync(user => !followingUsernamesList.Contains(user.UserName) && user.UserName != requestingUsername);

                // Assert
                actual.ShouldBeType<List<ApplicationUser>>();
                actual.ShouldBeEmpty();
            }
        }

        #endregion


        #region test with SearchBirdersToFollow predicate

        //user => user.NormalizedUserName.Contains(searchCriterion.ToUpper()) 
        //&& !followingUsernamesList.Contains(user.UserName)

        [Theory]
        [InlineData("2")]
        [InlineData("TestUser2")]
        [InlineData("Test")]
        [InlineData("User")]
        [InlineData("")]
        public async Task GetUsersAsync_SearchCriterion_ReturnsOneUser(string searchCriterion)
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                string requestingUsername = "TestUser1";
                string usernameToFollow = "TestUser2";

                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();  // int number of users?

                context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
                context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
                context.SaveChanges();
                context.Users.Count().ShouldEqual(2);
                IEnumerable<string> followingUsernamesList = new List<string> { requestingUsername };

                var userManager = SharedFunctions.InitialiseUserManager(context);

                // Act
                var actual = await userManager.GetUsersAsync(user => user.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingUsernamesList.Contains(user.UserName));

                // Assert
                actual.ShouldBeType<List<ApplicationUser>>();
                actual.Count().ShouldEqual(1);
            }
        }


        [Theory]
        [InlineData("1")]
        [InlineData("TestUser1")]
        public async Task GetUsersAsync_SearchCriterion_ReturnsNoUser(string searchCriterion)
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                string requestingUsername = "TestUser1";
                string usernameToFollow = "TestUser2";

                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //context.SeedDatabaseFourBooks();  // int number of users?

                context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
                context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
                context.SaveChanges();
                context.Users.Count().ShouldEqual(2);
                IEnumerable<string> followingUsernamesList = new List<string> { requestingUsername };

                var userManager = SharedFunctions.InitialiseUserManager(context);

                // Act
                var actual = await userManager.GetUsersAsync(user => user.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingUsernamesList.Contains(user.UserName));

                // Assert
                actual.ShouldBeType<List<ApplicationUser>>();
                actual.ShouldBeEmpty();
            }
        }

        #endregion


        [Fact]
        public async Task GetUsersAsync_ReturnsException_WhenArgumentIsNull()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                //IEnumerable<string> followersNotBeingFollowed;

                var userManager = SharedFunctions.InitialiseUserManager(context);

                // Act & Assert
                var ex = await Assert.ThrowsAsync<ArgumentException>(() => userManager.GetUsersAsync(null));
                Assert.Equal("The argument is null or empty (Parameter 'predicate')", ex.Message);
            }
        }
    }
}
