using Birder.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Helpers
{
    public static class NetworkHelpers
    {
        public static List<string> GetFollowersUserNames(ICollection<Network> followers)
        {
            // what if followers == null?
            return (from user in followers
                        select user.Follower.UserName).ToList();
        }

        public static List<string> GetFollowingUserNames(ICollection<Network> following)
        {
            // what if following == null?
            return (from user in following
                        select user.ApplicationUser.UserName).ToList();
        }

        public static IEnumerable<string> GetFollowersNotBeingFollowedUserNames(ApplicationUser loggedinUser)
        {
            // what if followers || following == null?
            var followersUsernamesList = GetFollowersUserNames(loggedinUser.Followers);
            var followingUsernamesList = GetFollowingUserNames(loggedinUser.Following);
            followingUsernamesList.Add(loggedinUser.UserName);
            return followersUsernamesList.Except(followingUsernamesList);
        }
    }
}
