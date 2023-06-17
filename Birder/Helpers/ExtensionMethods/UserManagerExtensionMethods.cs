using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Birder.Helpers;

public static class UserManagerExtensionMethods
{
    /// <summary>
    /// Returns a User with their collections of followers and following
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public static async Task<ApplicationUser> GetUserWithNetworkAsync(this UserManager<ApplicationUser> userManager,
        string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("The argument is null or empty", nameof(username));

        return await userManager.Users
            .Include(x => x.Followers)
                .ThenInclude(x => x.Follower)
            .Include(y => y.Following)
                .ThenInclude(r => r.ApplicationUser)
            .Where(x => x.UserName == username)
            .AsSplitQuery() // see: https://docs.microsoft.com/en-gb/ef/core/querying/single-split-queries
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Returns a list of Users which match the supplied predicate
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<ApplicationUser>> GetUsersAsync(this UserManager<ApplicationUser> userManager,
        Expression<Func<ApplicationUser, bool>> predicate)
    {
        if (predicate is null)
            throw new ArgumentException("The argument is null or empty", nameof(predicate));

        return await userManager.Users.Where(predicate).ToListAsync();
    }
}