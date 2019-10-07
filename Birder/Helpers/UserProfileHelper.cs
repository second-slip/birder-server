using Birder.Data.Model;
using Birder.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Helpers
{
    public static class UserProfileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestedUser"></param>
        /// <param name="requestingUser"></param>
        /// <returns></returns>
        public static bool UpdateIsFollowingProperty(ApplicationUser requestedUser, ApplicationUser requestingUser)
        {
            return requestedUser.Followers.Any(cus => cus.Follower.UserName == requestingUser.UserName);
        }

        
        public static IEnumerable<FollowingViewModel> UpdateFollowingCollection(IEnumerable<FollowingViewModel> following, ApplicationUser requestingUser) //, string loggedinUsername)
        {
            for (int i = 0; i < following.Count(); i++)
            {
                following.ElementAt(i).IsFollowing = requestingUser.Following.Any(cus => cus.ApplicationUser.UserName == following.ElementAt(i).UserName);
                following.ElementAt(i).IsOwnProfile = following.ElementAt(i).UserName == requestingUser.UserName;
            }

            return following;
        }

        /// <summary>
        /// Updates the requested user's 'Followers' collection (IsFollowing and IsOwnProfile properties)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="requestingUser"></param>
        /// <returns></returns>
        public static IEnumerable<FollowerViewModel> UpdateFollowersCollection(IEnumerable<FollowerViewModel> followers, ApplicationUser requestingUser) //, string loggedinUsername)
        {
            for (int i = 0; i < followers.Count(); i++)
            {
                followers.ElementAt(i).IsFollowing = requestingUser.Following.Any(cus => cus.ApplicationUser.UserName == followers.ElementAt(i).UserName);
                followers.ElementAt(i).IsOwnProfile = followers.ElementAt(i).UserName == requestingUser.UserName;
            }

            return followers;
        }
    }
}
