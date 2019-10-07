using Birder.Data.Model;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Helpers
{
    public static class UserProfileHelper
    {
        public static bool UpdateIsFollowingProperty(ApplicationUser requestedUser, ApplicationUser requestingUser)
        {
            if (requestedUser == null || requestingUser == null)
                throw new NullReferenceException("The requested user or requesting user is null");

            return requestedUser.Followers.Any(cus => cus.Follower.UserName == requestingUser.UserName);
        }

        public static IEnumerable<FollowingViewModel> UpdateFollowingCollection(IEnumerable<FollowingViewModel> following, ApplicationUser requestingUser)
        {
            if (following == null)
                throw new NullReferenceException("The following collection is null");

            if (requestingUser == null)
                throw new NullReferenceException("The requesting user is null");

            for (int i = 0; i < following.Count(); i++)
            {
                following.ElementAt(i).IsFollowing = requestingUser.Following.Any(cus => cus.ApplicationUser.UserName == following.ElementAt(i).UserName);
                following.ElementAt(i).IsOwnProfile = following.ElementAt(i).UserName == requestingUser.UserName;
            }

            return following;
        }

        public static IEnumerable<FollowerViewModel> UpdateFollowersCollection(IEnumerable<FollowerViewModel> followers, ApplicationUser requestingUser)
        {
            if (followers == null)
                throw new NullReferenceException("The followers collection is null");

            if (requestingUser == null)
                throw new NullReferenceException("The requesting user is null");

            for (int i = 0; i < followers.Count(); i++)
            {
                followers.ElementAt(i).IsFollowing = requestingUser.Following.Any(cus => cus.ApplicationUser.UserName == followers.ElementAt(i).UserName);
                followers.ElementAt(i).IsOwnProfile = followers.ElementAt(i).UserName == requestingUser.UserName;
            }

            return followers;
        }
    }
}
