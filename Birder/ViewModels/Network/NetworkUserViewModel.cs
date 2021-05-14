namespace Birder.ViewModels
{
    public class NetworkUserViewModel
    {
        public string UserName { get; set; }
        public string Avatar { get; set; }
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
