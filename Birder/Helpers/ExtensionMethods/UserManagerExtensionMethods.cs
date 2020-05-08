using Birder.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Birder.Helpers
{
    public static class UserManagerExtensionMethods
    {
        /// <summary>
        /// Returns a User with their collections of followers and following
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name=nameof(username)></param>
        /// <returns></returns>
        public static async Task<ApplicationUser> GetUserWithNetworkAsync(this UserManager<ApplicationUser> userManager, string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("The argument is null or empty", nameof(username));

            return await userManager.Users
                         .Include(x => x.Followers)
                             .ThenInclude(x => x.Follower)
                         .Include(y => y.Following)
                             .ThenInclude(r => r.ApplicationUser)
                         .Where(x => x.UserName == username)
                         .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="followersNotBeingFollowed"></param>
        /// <returns></returns>
        /// 
        // ****************** Hang On -- this just returns a list of users
        // ****************** generic GetUsers() method with predicate!!!!

        public static async Task<IEnumerable<ApplicationUser>> GetUsersAsync(this UserManager<ApplicationUser> userManager, Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await userManager.Users.Where(predicate).ToListAsync();
        }

        public static async Task<IEnumerable<ApplicationUser>> GetFollowersNotFollowedAsync(this UserManager<ApplicationUser> userManager, IEnumerable<string> followersNotBeingFollowed)
        {
            if (followersNotBeingFollowed is null)
                throw new ArgumentException("The argument is null or empty", nameof(followersNotBeingFollowed));

            return await userManager.Users.Where(users => followersNotBeingFollowed.Contains(users.UserName)).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="username"></param>
        /// <param name="followingList"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(this UserManager<ApplicationUser> userManager, string username, IEnumerable<string> followingList)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("The argument is null or empty", nameof(username));

            if (followingList is null)
                throw new ArgumentException("The argument is null or empty", nameof(followingList));
            // If user is following every follower

            return await userManager.Users.Where(users => !followingList.Contains(users.UserName) && users.UserName != username).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="searchCriterion"></param>
        /// <param name="followingList"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(this UserManager<ApplicationUser> userManager, string searchCriterion, IEnumerable<string> followingList)
        {
            if (string.IsNullOrEmpty(searchCriterion))
                throw new ArgumentException("The argument is null or empty", nameof(searchCriterion));

            if (followingList is null)
                throw new ArgumentException("The argument is null or empty", nameof(followingList));

            return await userManager.Users.Where(users => users.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingList.Contains(users.UserName)).ToListAsync();
        }
    }
}
