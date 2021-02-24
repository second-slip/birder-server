using System;

namespace Birder.ViewModels
{
    public class UserProfileViewModel
    {
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsOwnProfile { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public bool IsFollowing { get; set; }
        //public IEnumerable<FollowerViewModel> Followers { get; set; }
        //public IEnumerable<FollowingViewModel> Following { get; set; }
    }
}
