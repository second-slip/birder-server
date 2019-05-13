using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.ViewModels
{
    public class NetworkUserViewModel
    {
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsOwnProfile { get; set; }
    }

    public class FollowingViewModel : NetworkUserViewModel
    {
    }

    public class FollowerViewModel : NetworkUserViewModel
    {
    }
}
