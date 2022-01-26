namespace Birder.ViewModels
{
    public class UserProfileViewModel
    {
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsOwnProfile { get; set; }
        public ObservationAnalysisViewModel ObservationCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public bool IsFollowing { get; set; }
    }
}
