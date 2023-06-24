using TestSupport.EfHelpers;

namespace Birder.Tests.Data_Persistence;

public class NetworkRepoTests
{
    [Fact]
    public async Task Follow_Should_Update_Independent_And_Dependent_Users()
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("depUser"));
        context.Users.Add(SharedFunctions.CreateUser("indUser"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(2);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var depUser = await userManager.GetUserWithNetworkAsync("depUser");
        depUser.Followers.ShouldBeEmpty();
        depUser.Following.ShouldBeEmpty();

        var indUser = await userManager.GetUserWithNetworkAsync("indUser");
        indUser.Followers.ShouldBeEmpty();
        indUser.Following.ShouldBeEmpty();

        var service = new NetworkRepository(context);

        // Act
        service.Follow(depUser, indUser);
        context.SaveChanges();

        // Assert
        depUser.Followers.ShouldBeEmpty();
        depUser.Following.ShouldNotBeEmpty();
        depUser.Following.Count().ShouldEqual(1);

        indUser.Followers.ShouldNotBeEmpty();
        indUser.Followers.Count().ShouldEqual(1);
        indUser.Following.ShouldBeEmpty();

        Assert.Equal(depUser.Following.FirstOrDefault(), indUser.Followers.FirstOrDefault());
    }


    [Fact]
    public async Task Unfollow_Should_Update_Independent_And_Dependent_Users()
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("depUser"));
        context.Users.Add(SharedFunctions.CreateUser("indUser"));
        context.Users.Add(SharedFunctions.CreateUser("indUser2"));
        context.Users.Add(SharedFunctions.CreateUser("indUser3"));
        context.Users.Add(SharedFunctions.CreateUser("indUser4"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(5);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var depUser = await userManager.GetUserWithNetworkAsync("depUser");
        depUser.Followers.ShouldBeEmpty();
        depUser.Following.ShouldBeEmpty();

        var indUser = await userManager.GetUserWithNetworkAsync("indUser");
        indUser.Followers.ShouldBeEmpty();
        indUser.Following.ShouldBeEmpty();

        var indUser2 = await userManager.GetUserWithNetworkAsync("indUser2");
        indUser.Followers.ShouldBeEmpty();
        indUser.Following.ShouldBeEmpty();

        var indUser3 = await userManager.GetUserWithNetworkAsync("indUser3");
        indUser.Followers.ShouldBeEmpty();
        indUser.Following.ShouldBeEmpty();

        var indUser4 = await userManager.GetUserWithNetworkAsync("indUser4");
        indUser.Followers.ShouldBeEmpty();
        indUser.Following.ShouldBeEmpty();

        var service = new NetworkRepository(context);

        service.Follow(depUser, indUser);
        service.Follow(depUser, indUser2);
        service.Follow(depUser, indUser3);
        service.Follow(depUser, indUser4);
        context.SaveChanges();

        depUser.Followers.ShouldBeEmpty();
        depUser.Following.ShouldNotBeEmpty();
        depUser.Following.Count().ShouldEqual(4);

        indUser.Followers.ShouldNotBeEmpty();
        indUser.Followers.Count().ShouldEqual(1);
        indUser.Following.ShouldBeEmpty();

        var p = depUser.Following.Select(i => i.ApplicationUser.UserName).ToList();
        Assert.Equal(4, p.Count);
        Assert.Contains("indUser", p);
        Assert.Contains("indUser2", p);
        Assert.Contains("indUser3", p);
        Assert.Contains("indUser4", p);

        // Act

        service.Unfollow(depUser, indUser4);
        context.SaveChanges();

        depUser.Following.Count().ShouldEqual(3);
        p = depUser.Following.Select(i => i.ApplicationUser.UserName).ToList();
        Assert.Equal(3, p.Count);
        Assert.DoesNotContain("indUser4", p);
        indUser4.Followers.ShouldBeEmpty();

        service.Unfollow(depUser, indUser2);
        context.SaveChanges();

        depUser.Following.Count().ShouldEqual(2);
        p = depUser.Following.Select(i => i.ApplicationUser.UserName).ToList();
        Assert.Equal(2, p.Count);
        Assert.DoesNotContain("indUser2", p);
        indUser2.Followers.ShouldBeEmpty();

        service.Unfollow(depUser, indUser);
        context.SaveChanges();

        depUser.Following.Count().ShouldEqual(1);
        p = depUser.Following.Select(i => i.ApplicationUser.UserName).ToList();
        Assert.Single(p);
        Assert.DoesNotContain("indUser", p);
        indUser.Followers.ShouldBeEmpty();

        service.Unfollow(depUser, indUser3);
        context.SaveChanges();

        depUser.Following.Count().ShouldEqual(0);
        p = depUser.Following.Select(i => i.ApplicationUser.UserName).ToList();
        Assert.Empty(p);
        indUser3.Followers.ShouldBeEmpty();
    }

    [Fact]
    public async Task Follow_When_LoggedinUser_Is_Null_Returns_Argument_Exception()
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("depUser"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var user = await userManager.GetUserWithNetworkAsync("depUser");

        var service = new NetworkRepository(context);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => service.Follow(null, user));
        Assert.Equal("method argument is null or empty (Parameter 'loggedinUser')", ex.Message);
    }

    [Fact]
    public async Task Follow_When_UserToFollow_Is_Null_Returns_Argument_Exception()
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("depUser"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var user = await userManager.GetUserWithNetworkAsync("depUser");

        var service = new NetworkRepository(context);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => service.Follow(user, null));
        Assert.Equal("method argument is null or empty (Parameter 'userToFollow')", ex.Message);
    }

        [Fact]
    public async Task Unfollow_When_LoggedinUser_Is_Null_Returns_Argument_Exception()
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("depUser"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var user = await userManager.GetUserWithNetworkAsync("depUser");

        var service = new NetworkRepository(context);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => service.Unfollow(null, user));
        Assert.Equal("method argument is null or empty (Parameter 'loggedinUser')", ex.Message);
    }

    [Fact]
    public async Task Unfollow_When_UserToFollow_Is_Null_Returns_Argument_Exception()
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("depUser"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        var userManager = SharedFunctions.InitialiseUserManager(context);

        var user = await userManager.GetUserWithNetworkAsync("depUser");

        var service = new NetworkRepository(context);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => service.Unfollow(user, null));
        Assert.Equal("method argument is null or empty (Parameter 'userToUnfollow')", ex.Message);
    }
}