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
        /// Gobble
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static async Task<ApplicationUser> GetUserAndTheirNetworkAsync(this UserManager<ApplicationUser> userManager, string username)
        {
            return await userManager.Users
                         .Include(x => x.Followers)
                             .ThenInclude(x => x.Follower)
                         .Include(y => y.Following)
                             .ThenInclude(r => r.ApplicationUser)
                         .Where(x => x.UserName == username)
                         .FirstOrDefaultAsync();
        }
    }
}
