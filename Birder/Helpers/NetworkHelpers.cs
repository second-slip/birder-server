using Birder.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Helpers
{
    public static class NetworkHelpers
    {
        public static List<string> GetFollowersUserNames(ICollection<Network> followers)
        {
            return (from user in followers
                        select user.Follower.UserName).ToList();
        }

        public static List<string> GetFollowingUserNames(ICollection<Network> following)
        {
            return (from user in following
                        select user.ApplicationUser.UserName).ToList();
        }
    }
}
