namespace Birder.Data.Repository;

public interface INetworkRepository
{
    void Follow(ApplicationUser loggedinUser, ApplicationUser userToFollow);
    void Unfollow(ApplicationUser loggedinUser, ApplicationUser userToUnfollow);
}

public class NetworkRepository : INetworkRepository
{
    private readonly ApplicationDbContext _dbContext;

    public NetworkRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Follow(ApplicationUser loggedinUser, ApplicationUser userToFollow)
    {
        if (loggedinUser is null)
            throw new ArgumentException("method argument is null or empty", nameof(loggedinUser));
        if (userToFollow is null)
            throw new ArgumentException("method argument is null or empty", nameof(userToFollow));

        userToFollow.Followers.Add(new Network { Follower = loggedinUser });
    }

    public void Unfollow(ApplicationUser loggedinUser, ApplicationUser userToUnfollow)
    {
        if (loggedinUser is null)
            throw new ArgumentException("method argument is null or empty", nameof(loggedinUser));
        if (userToUnfollow is null)
            throw new ArgumentException("method argument is null or empty", nameof(userToUnfollow));

        var record = userToUnfollow.Followers.Where(i => i.ApplicationUser == userToUnfollow).FirstOrDefault();
        loggedinUser.Following.Remove(record);
        userToUnfollow.Followers.Remove(record);
    }
}

// public async Task<IEnumerable<Network>> GetFollowing(ApplicationUser user)
// {
//     var following = await _dbContext.Network
//         .AsQueryable()
//         .AsNoTracking()
//         .Include(x => x.Follower)
//             .ThenInclude(x => x.Followers)
//         .Where(x => x.Follower == user)
//         .ToListAsync();

//     return following;
// }

// public async Task<IEnumerable<Network>> GetFollowers(ApplicationUser user)
// {
//     var followers = await _dbContext.Network
//         .AsQueryable()
//         .AsNoTracking()
//         .Include(x => x.ApplicationUser)
//             .ThenInclude(x => x.Following)
//         .Where(x => x.ApplicationUser == user)
//         .ToListAsync();

//     return followers;
// }