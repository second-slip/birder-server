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
        /// Returns a User with their collections of followers and following
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static async Task<ApplicationUser> GetUserWithNetworkAsync(this UserManager<ApplicationUser> userManager, string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("The argument is null or empty", "username");

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
            if (followersNotBeingFollowed is null)
                throw new ArgumentException("The argument is null or empty", "followersNotBeingFollowed");

            return await userManager.Users.Where(users => followersNotBeingFollowed.Contains(users.UserName)).ToListAsync();
        }

        public static async Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(this UserManager<ApplicationUser> userManager, string username, IEnumerable<string> followingList)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("The argument is null or empty", "username");

            if (followingList is null)
                throw new ArgumentException("The argument is null or empty", "followingList");
            // If user is following every follower

            return await userManager.Users.Where(users => !followingList.Contains(users.UserName) && users.UserName != username).ToListAsync();
        }

        public static async Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(this UserManager<ApplicationUser> userManager, string searchCriterion, IEnumerable<string> followingList)
        {
            if (string.IsNullOrEmpty(searchCriterion))
                throw new ArgumentException("The argument is null or empty", "searchCriterion");

            if (followingList is null)
                throw new ArgumentException("The argument is null or empty", "followingList");

            return await userManager.Users.Where(users => users.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingList.Contains(users.UserName)).ToListAsync();
        }
    }
}
