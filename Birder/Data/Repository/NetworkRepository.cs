using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Birder.Data.Repository;

public class NetworkRepository : INetworkRepository
{
    private readonly ApplicationDbContext _dbContext;

    public NetworkRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Follow(ApplicationUser loggedinUser, ApplicationUser userToFollow)
    {
        userToFollow.Followers.Add(new Network { Follower = loggedinUser });
    }

    public void UnFollow(ApplicationUser loggedinUser, ApplicationUser userToUnfollow)
    {
        loggedinUser.Following.Remove(userToUnfollow.Followers.FirstOrDefault());
    }

    public async Task<IEnumerable<Network>> GetFollowing(ApplicationUser user)
    {
        var t = await _dbContext.Network
            .Include(x => x.Follower)
            .ThenInclude(x => x.Followers)
            .Where(x => x.Follower == user)
            .ToListAsync();

        return t;
    }

    public async Task<IEnumerable<Network>> GetFollowers(ApplicationUser user)
    {
        var t = await _dbContext.Network
            .Include(x => x.ApplicationUser)
            .ThenInclude(x => x.Following)
            .Where(x => x.ApplicationUser == user)
            .ToListAsync();

        return t;
    }

    //public async Task<ApplicationUser> GetUserAndNetworkAsync(string userName)
    //{
    //    return await _dbContext.Users
    //                 .Include(x => x.Followers)
    //                     .ThenInclude(x => x.Follower)
    //                 .Include(y => y.Following)
    //                     .ThenInclude(r => r.ApplicationUser)
    //                 .Where(x => x.UserName == userName)
    //                 .FirstOrDefaultAsync();
    //}

    //public async Task<IEnumerable<ApplicationUser>> GetFollowersNotFollowedAsync(ApplicationUser user, IEnumerable<string> followersNotBeingFollowed)
    //{
    //    // If followersNotBeingFollowed.Count() != 0
    //    return await _dbContext.Users.Where(users => followersNotBeingFollowed.Contains(users.UserName)).ToListAsync();
    //}

    //public async Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(ApplicationUser user, IEnumerable<string> followingList)
    //{
    //    // If user is following every follower
    //    return await _dbContext.Users.Where(users => !followingList.Contains(users.UserName) && users.UserName != user.UserName).ToListAsync();
    //}

    //public async Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(ApplicationUser user, string searchCriterion, IEnumerable<string> followingList)
    //{
    //    return await _dbContext.Users.Where(users => users.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingList.Contains(users.UserName)).ToListAsync();
    //}
}

public interface INetworkRepository
{
    void Follow(ApplicationUser loggedinUser, ApplicationUser userToFollow);
    void UnFollow(ApplicationUser loggedinUser, ApplicationUser userToUnfollow);

    Task<IEnumerable<Network>> GetFollowers(ApplicationUser user);
    Task<IEnumerable<Network>> GetFollowing(ApplicationUser user);
    //Task<IEnumerable<ApplicationUser>> GetFollowersNotFollowedAsync(ApplicationUser user, IEnumerable<string> followersNotBeingFollowed);
    //Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(ApplicationUser user, IEnumerable<string> followingList);
    //Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(ApplicationUser user, string searchCriterion, IEnumerable<string> followingList);
}