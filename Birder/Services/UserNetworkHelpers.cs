namespace Birder.Helpers;

public interface IUserNetworkHelpers
{
    List<string> GetFollowersUserNames(ICollection<Network> followers);
    List<string> GetFollowingUserNames(ICollection<Network> following);
    IEnumerable<string> GetFollowersNotBeingFollowedUserNames(ApplicationUser loggedinUser);
    bool UpdateIsFollowing(string requestingUsername, ICollection<Network> requestedUsersFollowing);
    bool UpdateIsFollowingProperty(string requestingUsername, ICollection<Network> requestedUsersFollowers);
    IEnumerable<FollowingViewModel> SetupFollowingCollection(ApplicationUser requestingUser, IEnumerable<FollowingViewModel> following);
    IEnumerable<FollowerViewModel> SetupFollowersCollection(ApplicationUser requestingUser, IEnumerable<FollowerViewModel> followers);
}

public class UserNetworkHelpers : IUserNetworkHelpers
{
    public List<string> GetFollowersUserNames(ICollection<Network> followers)
    {
        if (followers is null)
            throw new NullReferenceException("The followers collection is null");

        return (from user in followers
                select user.Follower.UserName).ToList();
    }

    public List<string> GetFollowingUserNames(ICollection<Network> following)
    {
        if (following is null)
            throw new NullReferenceException("The following collection is null");

        return (from user in following
                select user.ApplicationUser.UserName).ToList();
    }

    public IEnumerable<string> GetFollowersNotBeingFollowedUserNames(ApplicationUser loggedinUser)
    {
        if (loggedinUser is null)
            throw new NullReferenceException("The user is null");

        List<string> followersUsernamesList = GetFollowersUserNames(loggedinUser.Followers);
        List<string> followingUsernamesList = GetFollowingUserNames(loggedinUser.Following);
        followingUsernamesList.Add(loggedinUser.UserName); //include own user name

        return followersUsernamesList.Except(followingUsernamesList);
    }

    public bool UpdateIsFollowing(string requestingUsername, ICollection<Network> requestedUsersFollowing)
    {
        if (string.IsNullOrEmpty(requestingUsername))
            throw new ArgumentNullException(nameof(requestingUsername), "The requestingUsername is null");

        if (requestedUsersFollowing is null)
            throw new ArgumentNullException(nameof(requestedUsersFollowing), "The following collection is null");

        return requestedUsersFollowing.Any(cus => cus.ApplicationUser.UserName == requestingUsername);
    }

    public bool UpdateIsFollowingProperty(string requestingUsername, ICollection<Network> requestedUsersFollowers)
    {
        if (string.IsNullOrEmpty(requestingUsername))
            throw new ArgumentNullException(nameof(requestingUsername), "The requestingUsername is null");

        if (requestedUsersFollowers is null)
            throw new ArgumentNullException(nameof(requestedUsersFollowers), "The followers collection is null");

        return requestedUsersFollowers.Any(cus => cus.Follower.UserName == requestingUsername);
    }

    public IEnumerable<FollowingViewModel> SetupFollowingCollection(ApplicationUser requestingUser, IEnumerable<FollowingViewModel> following)
    {
        if (following is null)
            throw new ArgumentNullException(nameof(following), "The following collection is null");

        if (requestingUser is null)
            throw new ArgumentNullException(nameof(requestingUser), "The requesting user is null");

        for (int i = 0; i < following.Count(); i++)
        {
            following.ElementAt(i).IsFollowing = requestingUser.Following.Any(cus => cus.ApplicationUser.UserName == following.ElementAt(i).UserName);
            following.ElementAt(i).IsOwnProfile = following.ElementAt(i).UserName == requestingUser.UserName;
        }

        return following;
    }

    public IEnumerable<FollowerViewModel> SetupFollowersCollection(ApplicationUser requestingUser, IEnumerable<FollowerViewModel> followers)
    {
        if (followers is null)
            throw new ArgumentNullException(nameof(followers), "The followers collection is null");

        if (requestingUser is null)
            throw new ArgumentNullException(nameof(requestingUser), "The requesting user is null");

        for (int i = 0; i < followers.Count(); i++)
        {
            followers.ElementAt(i).IsFollowing = requestingUser.Following.Any(cus => cus.ApplicationUser.UserName == followers.ElementAt(i).UserName);
            followers.ElementAt(i).IsOwnProfile = followers.ElementAt(i).UserName == requestingUser.UserName;
        }

        return followers;
    }
}