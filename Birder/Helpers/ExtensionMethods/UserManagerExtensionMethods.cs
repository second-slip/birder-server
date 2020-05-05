using Birder.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Helpers
{
    public static class UserManagerExtensionMethods
    {
        /// <summary>
        /// Gobble, gobble...
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static async Task<ApplicationUser> GetUserWithNetworkAsync(this UserManager<ApplicationUser> userManager, string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new NullReferenceException("The username argument is null or empty");

            return await userManager.Users
                         .Include(x => x.Followers)
                             .ThenInclude(x => x.Follower)
                         .Include(y => y.Following)
                             .ThenInclude(r => r.ApplicationUser)
                         .Where(x => x.UserName == username)
                         .FirstOrDefaultAsync();
        }

        public static async Task<IEnumerable<ApplicationUser>> GetFollowersNotFollowedAsync(this UserManager<ApplicationUser> userManager, IEnumerable<string> followersNotBeingFollowed)
        {
            // If followersNotBeingFollowed.Count() != 0
            //return await _dbContext.Users.Where(users => followersNotBeingFollowed.Contains(users.UserName)).ToListAsync();
            return await userManager.Users.Where(users => followersNotBeingFollowed.Contains(users.UserName)).ToListAsync();
        }

        public static async Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(this UserManager<ApplicationUser> userManager, string username, IEnumerable<string> followingList)
        {
            // If user is following every follower
            //return await _dbContext.Users.Where(users => !followingList.Contains(users.UserName) && users.UserName != user.UserName).ToListAsync();
            return await userManager.Users.Where(users => !followingList.Contains(users.UserName) && users.UserName != username).ToListAsync();
        }

        public static async Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(this UserManager<ApplicationUser> userManager, string searchCriterion, IEnumerable<string> followingList)
        {
            //return await _dbContext.Users.Where(users => users.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingList.Contains(users.UserName)).ToListAsync();
            return await userManager.Users.Where(users => users.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingList.Contains(users.UserName)).ToListAsync();
        }
    }
}
