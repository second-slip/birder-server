using Birder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
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

        public async Task<ApplicationUser> GetUserAndNetworkAsync(string userName)
        {
            return await _dbContext.Users
                         .Include(x => x.Followers)
                             .ThenInclude(x => x.Follower)
                         .Include(y => y.Following)
                             .ThenInclude(r => r.ApplicationUser)
                         .Where(x => x.UserName == userName)
                         .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetFollowersNotFollowedAsync(ApplicationUser user, IEnumerable<string> followersNotBeingFollowed)
        {
            // If followersNotBeingFollowed.Count() != 0
            return await _dbContext.Users.Where(users => followersNotBeingFollowed.Contains(users.UserName)).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(ApplicationUser user, IEnumerable<string> followingList)
        {
            // If user is following every follower
            return await _dbContext.Users.Where(users => !followingList.Contains(users.UserName) && users.UserName != user.UserName).ToListAsync();
        }


        public async Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(ApplicationUser user, string searchCriterion, IEnumerable<string> followingList)
        {
            return await _dbContext.Users.Where(users => users.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingList.Contains(users.UserName)).ToListAsync();
        }
    }
}
