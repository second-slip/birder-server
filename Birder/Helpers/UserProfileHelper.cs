using Birder.Data.Model;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Helpers
{
    public static class UserProfileHelper
    {

        public static UserProfileViewModel UpdateFollowingCollection(UserProfileViewModel viewModel, ApplicationUser loggedinUser, string loggedinUsername)
        {
            for (int i = 0; i < viewModel.Following.Count(); i++)
            {
                viewModel.Following.ElementAt(i).IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == viewModel.Following.ElementAt(i).UserName);
                viewModel.Following.ElementAt(i).IsOwnProfile = viewModel.Following.ElementAt(i).UserName == loggedinUsername;
            }

            return viewModel;
        }

        public static UserProfileViewModel UpdateFollowersCollection(UserProfileViewModel viewModel, ApplicationUser loggedinUser, string loggedinUsername)
        {
            for (int i = 0; i < viewModel.Followers.Count(); i++)
            {
                viewModel.Followers.ElementAt(i).IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == viewModel.Followers.ElementAt(i).UserName);
                viewModel.Followers.ElementAt(i).IsOwnProfile = viewModel.Followers.ElementAt(i).UserName == loggedinUsername;
            }

            return viewModel;
        }
    }
}
