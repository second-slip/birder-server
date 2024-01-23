using Microsoft.EntityFrameworkCore;
using TestSupport.EfHelpers;

namespace Birder.Tests.HelpersTests;

public class GetUsersAsyncTests
{
    public GetUsersAsyncTests() { }


    #region test with FollowersNotFollowed predicate

    [Fact]
    public async Task GetUsersAsync_FollowersNotFollowedEmptyCollectionArgument_ReturnsNoUsers()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();

        using var context = new ApplicationDbContext(options);


        context.Database.EnsureCreated();
        //temporary increase timeout only for one Context instance.
        //context.Database.SetCommandTimeout(TimeSpan.FromMinutes(1));
        // Arrange
        string requestingUsername = "TestUser1";
        string usernameToFollow = "TestUser2";
        IEnumerable<string> followersNotBeingFollowed = new List<string>();

        context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
        context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        // Act
        var actual = await userManager.GetUsersAsync(user => followersNotBeingFollowed.Contains(user.UserName));

        // Assert
        actual.ShouldBeType<List<ApplicationUser>>();
        actual.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetUsersAsync_OneFollowerNotFollowed_ReturnsOneUser()
    {
        // Arrange
        string requestingUsername = "TestUser1";
        string followerUsername = "TestUser2";
        IEnumerable<string> followersNotBeingFollowed = new List<string> { followerUsername };

        // when handling Network object, need to use sql database not sqlite in-memory
        var options = this.CreateUniqueMethodOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureClean();
        //temporary increase timeout only for one Context instance.
        //context.Database.SetCommandTimeout(TimeSpan.FromMinutes(1));

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
            ApplicationUserId = requestingUser.Id,
            Follower = follower,
            FollowerId = follower.Id
        });

        context.SaveChanges();
        context.Network.Count().ShouldEqual(1);

        // Act
        var actual = await userManager.GetUsersAsync(user => followersNotBeingFollowed.Contains(user.UserName));

        // Assert
        actual.ShouldBeType<List<ApplicationUser>>();
        actual.Count().ShouldEqual(1);
        actual.FirstOrDefault().UserName.ShouldEqual(followerUsername);
    }

    #endregion


    #region test with SuggestedBirdersToFollow predicate

    //user => !followingUsernamesList.Contains(user.UserName) 
    // && user.UserName != requestingUser.UserName

    [Fact]
    public async Task GetUsersAsync_OneSuggestedUserToFollow_ReturnsUser()
    {
        // Arrange
        string requestingUsername = "TestUser1";
        string usernameToFollow = "TestUser2";
        IEnumerable<string> followingUsernamesList = new List<string>();

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        //temporary increase timeout only for one Context instance.
        //context.Database.SetCommandTimeout(TimeSpan.FromMinutes(1));

        context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
        context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        // Act
        var actual = await userManager.GetUsersAsync(user => !followingUsernamesList.Contains(user.UserName) && user.UserName != requestingUsername);

        // Assert
        actual.ShouldBeType<List<ApplicationUser>>();
        actual.Count().ShouldEqual(1);
    }


    [Fact]
    public async Task GetUsersAsync_NoSuggestedUserToFollow_ReturnsNoUser()
    {
        // Arrange
        string requestingUsername = "TestUser1";
        string usernameToFollow = "TestUser2";
        IEnumerable<string> followingUsernamesList = new List<string> { usernameToFollow };

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        //temporary increase timeout only for one Context instance.
        //context.Database.SetCommandTimeout(TimeSpan.FromMinutes(1));

        context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
        context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);


        var userManager = SharedFunctions.InitialiseUserManager(context);

        // Act
        var actual = await userManager.GetUsersAsync(user => !followingUsernamesList.Contains(user.UserName) && user.UserName != requestingUsername);

        // Assert
        actual.ShouldBeType<List<ApplicationUser>>();
        actual.ShouldBeEmpty();
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
        // Arrange
        string requestingUsername = "TestUser1";
        string usernameToFollow = "TestUser2";
        IEnumerable<string> followingUsernamesList = new List<string> { requestingUsername };

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        //temporary increase timeout only for one Context instance.
        //context.Database.SetCommandTimeout(TimeSpan.FromMinutes(1));

        context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
        context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        // Act
        var actual = await userManager.GetUsersAsync(user => user.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingUsernamesList.Contains(user.UserName));

        // Assert
        actual.ShouldBeType<List<ApplicationUser>>();
        actual.Count().ShouldEqual(1);
    }


    [Theory]
    [InlineData("1")]
    [InlineData("TestUser1")]
    public async Task GetUsersAsync_SearchCriterion_ReturnsNoUser(string searchCriterion)
    {
        // Arrange
        string requestingUsername = "TestUser1";
        string usernameToFollow = "TestUser2";
        IEnumerable<string> followingUsernamesList = new List<string> { requestingUsername };

        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        //temporary increase timeout only for one Context instance.
        //context.Database.SetCommandTimeout(TimeSpan.FromMinutes(1));

        context.Users.Add(SharedFunctions.CreateUser(requestingUsername));
        context.Users.Add(SharedFunctions.CreateUser(usernameToFollow));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        // Act
        var actual = await userManager.GetUsersAsync(user => user.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingUsernamesList.Contains(user.UserName));

        // Assert
        actual.ShouldBeType<List<ApplicationUser>>();
        actual.ShouldBeEmpty();
    }

    #endregion


    [Fact]
    public async Task GetUsersAsync_ReturnsException_WhenArgumentIsNull()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var userManager = SharedFunctions.InitialiseUserManager(context);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => userManager.GetUsersAsync(null));
        Assert.Equal("The argument is null or empty (Parameter 'predicate')", ex.Message);
    }
}